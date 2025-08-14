using dotnet.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Data
{
    /// <summary>
    /// EF Core database context for TaskFlow API.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        // PUBLIC_INTERFACE
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // PUBLIC_INTERFACE
        public DbSet<User> Users => Set<User>();

        // PUBLIC_INTERFACE
        public DbSet<TaskItem> Tasks => Set<TaskItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.UserName).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.UserName).HasMaxLength(64).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
                entity.Property(u => u.PasswordHash).IsRequired();
                entity.Property(u => u.PasswordSalt).IsRequired();
            });

            // Task configuration
            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.Property(t => t.Title).HasMaxLength(200).IsRequired();
                entity.Property(t => t.Status).HasConversion<int>();
                entity.HasOne(t => t.AssignedToUser)
                      .WithMany()
                      .HasForeignKey(t => t.AssignedToUserId)
                      .OnDelete(DeleteBehavior.SetNull);
            });
        }
    }
}
