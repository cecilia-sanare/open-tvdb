using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Database;

public class OpenTVDBContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Series> Series => Set<Series>();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditInfo();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAuditInfo()
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Created = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Updated = now;
            }
        }
    }
}