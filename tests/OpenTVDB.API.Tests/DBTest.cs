using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;

namespace OpenTVDB.API.Tests;

public class DBTest : IAsyncLifetime
{
    protected readonly OpenTVDBContext Context;
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
        var (series, movies) = await (
            Context.Series.ToArrayAsync(),
            Context.Movies.ToArrayAsync()
        );

        var entities = series.Cast<object>().Concat(movies).ToArray();

        if (entities.Length > 0)
        {
            Context.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }

        await Context.DisposeAsync();
    }
}
