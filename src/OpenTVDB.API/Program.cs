using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;
using OpenTVDB.API.Repositories;
using OpenTVDB.API.Services;
using Scalar.AspNetCore;
using OpenTVDB.API.Enums;

namespace OpenTVDB.API;

// Not really testable
[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var isTestEnvironment = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName != null && a.FullName.StartsWith("xunit"));

        var builder = WebApplication.CreateBuilder(args);

        if (isTestEnvironment)
        {
            builder.Configuration.Sources.Clear();
            builder.Configuration.AddJsonFile("appsettings.Testing.json");
        }

        var config = builder.Configuration.Get<Config>();

        if (config == null) throw new Exception("Failed to load config.");

        ConfigureServices(config, builder.Services);

        var app = ConfigureApp(builder.Build());

        // Run migrations on startup
        if (config.Database.Type != DatabaseType.InMemory)
        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OpenTVDBContext>();
            db.Database.Migrate();
        }

        app.Run();
    }

    public static void ConfigureServices(Config config, IServiceCollection services)
    {
        // Configure how controllers are handled
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddHealthChecks();

        services.AddDbContext<OpenTVDBContext>(options =>
        {
            switch (config.Database.Type)
            {
                case DatabaseType.InMemory:
                    options.UseInMemoryDatabase(config.Database.ConnectionString);
                    break;
                case DatabaseType.Sqlite:
                    options.UseSqlite(config.Database.ConnectionString);
                    break;
                default:
                    throw new Exception($"Unknown database type. {config.Database.Type}");
            }
        });

        // Add our services
        services
            .AddScoped<ISeriesService, SeriesService>()
            .AddScoped<IMovieService, MovieService>()
            .AddScoped<ISearchItemService, SearchItemService>();

        // Add our Repositories
        services
            .AddScoped<ISeriesRepository, SeriesRepository>()
            .AddScoped<IMovieRepository, MovieRepository>()
            .AddScoped<ISearchItemRepository, SearchItemRepository>();

        // Generate OpenAPI Output
        services.AddOpenApiDocument(configure =>
        {
            configure.Title = "OpenTVDB API";
            configure.Description = "The documentation for the OpenTVDB API.";
            configure.DocumentProcessors.Add(new CustomSchemaIgnoreDocumentProcessor());
        });
    }

    public static WebApplication ConfigureApp(WebApplication app)
    {
        app.MapControllers();
        app.MapHealthChecks("/api/health");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseOpenApi(options =>
            {
                options.Path = "/openapi/v1.json";
            });

            app.MapScalarApiReference(options =>
                options.WithTitle("OpenTVDB API")
                    .WithTheme(ScalarTheme.BluePlanet)
                    .WithDarkModeToggle(false)
            );
        }
        else
        {
            app.UseHttpsRedirection();
        }

        return app;
    }
}
