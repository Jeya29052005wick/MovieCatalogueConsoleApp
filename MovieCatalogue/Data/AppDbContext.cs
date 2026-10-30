
using Microsoft.EntityFrameworkCore;
using MovieCatalogue.Models;
using MovieCatalogueEFCore.Models;
using System.Text.Json;

namespace MovieCatalogue.Data;

public class AppDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public DbSet<AuditLog> AuditLogs { get; set; }

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

        // Movie Configuration
        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(m => m.Language)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(m => m.DurationMinutes)
                .IsRequired();

            entity.Property(m => m.ReleaseYear)
                .IsRequired();
        });

        // AuditLog Configuration
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.EntityName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.EntityId)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(a => a.Action)
                .IsRequired()
                .HasMaxLength(20);

            entity.Property(a => a.PerformedBy)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(a => a.PerformedAt)
                .IsRequired();

            entity.HasIndex(a => new
            {
                a.EntityName,
                a.EntityId
            });
        });
    }

    // Automatic Audit Logging
    public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        ChangeTracker.DetectChanges();

        var entries = ChangeTracker.Entries()
            .Where(e =>
                e.Entity is not AuditLog &&
                (e.State == EntityState.Added ||
                 e.State == EntityState.Modified ||
                 e.State == EntityState.Deleted))
            .ToList();

        // No changes to audit
        if (entries.Count == 0)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        var auditEntries = new List<(Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry Entry, AuditLog Log)>();

        foreach (var entry in entries)
        {
            var oldValues = new Dictionary<string, object?>();
            var newValues = new Dictionary<string, object?>();

            foreach (var property in entry.Properties)
            {
                if (property.Metadata.IsPrimaryKey())
                    continue;

                switch (entry.State)
                {
                    case EntityState.Added:
                        newValues[property.Metadata.Name] =
                            property.CurrentValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            oldValues[property.Metadata.Name] =
                                property.OriginalValue;

                            newValues[property.Metadata.Name] =
                                property.CurrentValue;
                        }
                        break;

                    case EntityState.Deleted:
                        oldValues[property.Metadata.Name] =
                            property.OriginalValue;
                        break;
                }
            }

            string action = entry.State switch
            {
                EntityState.Added => "Created",
                EntityState.Modified => "Updated",
                EntityState.Deleted => "Deleted",
                _ => "Unknown"
            };

            var auditLog = new AuditLog
            {
                EntityName = entry.Metadata.ClrType.Name,
                Action = action,
                OldValues = oldValues.Count > 0
                    ? JsonSerializer.Serialize(oldValues)
                    : null,
                NewValues = newValues.Count > 0
                    ? JsonSerializer.Serialize(newValues)
                    : null,
                PerformedBy = "Admin",
                PerformedAt = DateTime.UtcNow
            };

            auditEntries.Add((entry, auditLog));
        }

        // Use a transaction so movie changes and audit records
        // are committed or rolled back together.
        var transaction = Database.CurrentTransaction == null
            ? await Database.BeginTransactionAsync(cancellationToken)
            : null;

        try
        {
            // First save: Save actual Movie changes.
            int affectedRows = await base.SaveChangesAsync(
                cancellationToken);

            // Get generated IDs after the first save.
            foreach (var item in auditEntries)
            {
                var primaryKey = item.Entry.Metadata.FindPrimaryKey();

                if (primaryKey == null || primaryKey.Properties.Count != 1)
                {
                    throw new InvalidOperationException(
                        "Audit logging requires a single-column primary key.");
                }

                var keyProperty = primaryKey.Properties[0];

                var keyValue = keyProperty.PropertyInfo?.GetValue(
                    item.Entry.Entity);

                if (keyValue == null)
                {
                    keyValue = item.Entry.Property(keyProperty.Name).CurrentValue;
                }

                item.Log.EntityId = Convert.ToString(
                    keyValue,
                    System.Globalization.CultureInfo.InvariantCulture
                ) ?? string.Empty;
            }

            // Second save: Save audit records.
            AuditLogs.AddRange(
                auditEntries.Select(x => x.Log));

            await base.SaveChangesAsync(cancellationToken);

            if (transaction != null)
            {
                await transaction.CommitAsync(cancellationToken);
            }

            return affectedRows;
        }
        catch
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
            }

            throw;
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
        }
    }
}
