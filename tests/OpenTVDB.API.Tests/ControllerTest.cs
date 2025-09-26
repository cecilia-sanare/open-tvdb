using Microsoft.Extensions.DependencyInjection;
using OpenTVDB.API.Database;

namespace OpenTVDB.API.Tests;

public class ControllerTest(WebApplicationFactoryTest factory) : IClassFixture<WebApplicationFactoryTest>, IAsyncLifetime
{
    protected OpenTVDBContext Context = null!;
    protected DateTime TestStart = DateTime.UtcNow;
    
    public Task InitializeAsync()
    {
        TestStart = DateTime.UtcNow;
        
        var scope = factory.Services.CreateScope();
        Context = scope.ServiceProvider.GetRequiredService<OpenTVDBContext>();

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