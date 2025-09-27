using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenTVDB.API.Database;

namespace OpenTVDB.API.Tests;

public class DatabaseTest : IClassFixture<WebApplicationFactoryTest>, IAsyncLifetime
{
    protected readonly OpenTVDBContext Context;
    protected readonly WebApplicationFactoryTest Factory;

    protected DateTime TestStart = DateTime.UtcNow;

    public DatabaseTest(WebApplicationFactoryTest factory)
    {
        Factory = factory;

        var scope = Factory.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<OpenTVDBContext>();
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
