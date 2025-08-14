using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using dotnet.Data;
using dotnet.Services.Interfaces;
using dotnet.Services;
using dotnet.Services.Security;
using dotnet.Repositories.Interfaces;
using dotnet.Repositories;
using dotnet.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers and API Explorer
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure OpenAPI/NSwag with metadata and JWT security scheme
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "TaskFlow API";
    config.Version = "v1";
    config.Description = "TaskFlow API provides endpoints for creating, updating, and managing tasks with JWT-based auth.";
    config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the text box: Bearer {your JWT token}."
    });
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Bind JwtSettings from configuration/environment variables
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

// Configure EF Core SQL Server using connection string from appsettings or environment
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
if (string.IsNullOrWhiteSpace(connectionString))
{
    // Log a warning; database operations will fail until this is configured
    Console.WriteLine("WARNING: No SQL Server connection string configured. Set ConnectionStrings:DefaultConnection or SQLSERVER_CONNECTION_STRING environment variable.");
}
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Register application services (DI)
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? Environment.GetEnvironmentVariable("Jwt__Key") ?? Environment.GetEnvironmentVariable("JWT__Key") ?? "";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? Environment.GetEnvironmentVariable("Jwt__Issuer") ?? Environment.GetEnvironmentVariable("JWT__Issuer");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? Environment.GetEnvironmentVariable("Jwt__Audience") ?? Environment.GetEnvironmentVariable("JWT__Audience");

if (string.IsNullOrWhiteSpace(jwtKey))
{
    Console.WriteLine("WARNING: JWT signing key is not configured. Set Jwt:Key or environment variable JWT__Key.");
}

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // For development; enable in production behind HTTPS
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
            ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(string.IsNullOrEmpty(jwtKey) ? "temporary-development-key-change-me" : jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Apply pending migrations or create database (initialization)
// For initial implementation, EnsureCreated avoids requiring migrations
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization error: {ex.Message}");
    }
}

// Middleware
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

// Configure OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

// Health check endpoint
app.MapGet("/", () => new { message = "Healthy" })
   .WithTags("Health");

// Map Controllers
app.MapControllers();

app.Run();