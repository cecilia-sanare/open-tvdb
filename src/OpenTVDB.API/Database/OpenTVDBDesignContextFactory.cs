using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace OpenTVDB.API.Database;

[ExcludeFromCodeCoverage]
public class OpenTVDBDesignContextFactory : IDesignTimeDbContextFactory<OpenTVDBContext>
{
    public OpenTVDBContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<OpenTVDBContext>();

        // Set your connection string here manually for design-time
        optionsBuilder.UseSqlite("Data Source=./open-tvdb.db");

        return new OpenTVDBContext(optionsBuilder.Options);
    }
}
