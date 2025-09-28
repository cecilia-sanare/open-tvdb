using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Database;

public class OpenTVDBContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Series> Series => Set<Series>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<SearchItem> SearchItems => Set<SearchItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SearchItem>().ToView("View_SearchItems");
    }

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
