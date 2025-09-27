using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Namotion.Reflection;
using OpenTVDB.API.Database;

namespace OpenTVDB.API.Tests;

public class ControllerTest(WebApplicationFactoryTest factory)
    : IClassFixture<WebApplicationFactoryTest>, IAsyncLifetime
{
    protected OpenTVDBContext Context = null!;
    protected DateTime TestStart = DateTime.UtcNow;
    protected WebApplicationFactoryTest Factory = factory;

    public Task InitializeAsync()
    {
        TestStart = DateTime.UtcNow;

        var scope = Factory.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<OpenTVDBContext>();

        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        object[] series = await Context.Series.ToArrayAsync();
        object[] movies = await Context.Movies.ToArrayAsync();
        var entities = series.Concat(movies).ToList();

        if (entities.Count > 0)
        {
            Context.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }

        await Context.DisposeAsync();
    }
}
