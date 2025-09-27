using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;
using OpenTVDB.API.Repositories;
using OpenTVDB.API.Services;
using Scalar.AspNetCore;
using OpenTVDB.API;

namespace OpenTVDB.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        ConfigureServices(builder.Services);

        var app = ConfigureApp(builder.Build());

        app.Run();
    }

    public static void ConfigureServices(IServiceCollection services)
    {
        // Configure how controllers are handled
        services.AddControllers();
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services.AddDbContext<OpenTVDBContext>(options => options.UseInMemoryDatabase("open-tvdb"));

        // Add our services
        services
            .AddScoped<ISeriesService, SeriesService>()
            .AddScoped<IMovieService, MovieService>();

        // Add our Repositories
        services
            .AddScoped<ISeriesRepository, SeriesRepository>()
            .AddScoped<IMovieRepository, MovieRepository>();

        // Generate OpenAPI Output
        services.AddOpenApiDocument(configure =>
        {
            configure.DocumentProcessors.Add(new CustomSchemaIgnoreDocumentProcessor());
        });
    }

    public static WebApplication ConfigureApp(WebApplication app)
    {
        app.MapControllers();

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
