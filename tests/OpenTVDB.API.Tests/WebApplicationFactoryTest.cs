using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OpenTVDB.API.Database;

namespace OpenTVDB.API.Tests;

public class WebApplicationFactoryTest : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DbContext registration
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<OpenTVDBContext>)
            );

            if (descriptor != null)
                services.Remove(descriptor);

            // Add new DbContext with in-memory database
            var databaseName = Guid.NewGuid().ToString();
            services.AddDbContext<OpenTVDBContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName);
            });

            // Optionally: Ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OpenTVDBContext>();
            db.Database.EnsureCreated();
        });
    }
}