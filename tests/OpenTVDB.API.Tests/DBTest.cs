using Bogus;
using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;

namespace OpenTVDB.API.Tests;

public class DBTest : IAsyncLifetime
{
    protected OpenTVDBContext Context = null!;
    protected DateTime TestStart = DateTime.UtcNow;

    protected DBTest()
    {
        var builder = new DbContextOptionsBuilder<OpenTVDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

        Context = new OpenTVDBContext(builder.Options);
    }
    
    public Task InitializeAsync()
    {
        TestStart = DateTime.UtcNow;
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        var entities = Context.ChangeTracker.Entries().Select(e => e.Entity);
        Context.RemoveRange(entities);
        await Context.SaveChangesAsync();
        await Context.DisposeAsync();
    }
}