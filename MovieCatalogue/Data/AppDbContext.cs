
using Microsoft.EntityFrameworkCore;
using MovieCatalogueEFCore.Models;

namespace MovieCatalogueEFCore.Data;

public class AppDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost\\SQLEXPRESS;" +
                "Database=MovieCatalogueDB;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            // Primary Key
            entity.HasKey(m => m.Id);

            // Title Configuration
            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);

            // Genre Configuration
            entity.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(50);

            // Language Configuration
            entity.Property(m => m.Language)
                .IsRequired()
                .HasMaxLength(50);

            // Duration Configuration
            entity.Property(m => m.DurationMinutes)
                .IsRequired();

            // Release Year Configuration
            entity.Property(m => m.ReleaseYear)
                .IsRequired();
        });
    }
}
