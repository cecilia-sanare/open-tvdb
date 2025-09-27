using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;
using OpenTVDB.API.Repositories;
using OpenTVDB.API.Services;
using Scalar.AspNetCore;
using OpenTVDB.API;

void ConfigureServices(IServiceCollection services)
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

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(options => { options.Path = "/openapi/v1.json"; });

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

app.Run();

public partial class Program
{
}
