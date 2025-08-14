# TaskFlow API

TaskFlow API is a .NET 8 Web API providing endpoints to create, update, and manage tasks with JWT-based authentication. It follows a clean architecture with Controllers, Services, Repositories, and Models. Persistence is implemented via Entity Framework Core with SQL Server.

## Run locally

1. Copy `api_backend/.env.example` to `api_backend/.env` (or export variables) and set:
   - `SQLSERVER_CONNECTION_STRING`
   - `JWT__Key`
   - `JWT__Issuer`
   - `JWT__Audience`

2. Start the API:
   - `dotnet run` from `api_backend/`

3. API Docs:
   - Open `http://localhost:3001/docs`

Notes:
- The database is initialized with `EnsureCreated()` for first run. For production, use EF Core migrations.

## Project structure (api_backend)

- Controllers/ — Web API controllers
- Data/ — EF Core DbContext
- DTOs/ — Request/Response models
- Models/ — Domain entities
- Repositories/ — Data access abstractions/implementations
- Services/ — Business logic and security
- Settings/ — Configuration binding classes
