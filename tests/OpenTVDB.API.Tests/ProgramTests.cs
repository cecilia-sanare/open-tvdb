using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using OpenTVDB.API.Repositories;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Tests;

public class ProgramTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public void Services_ShouldBeRegisteredCorrectly()
    {
        using var scope = factory.Services.CreateScope();
        var provider = scope.ServiceProvider;

        provider.GetService<ISeriesService>().Should().NotBeNull();
        provider.GetService<IMovieService>().Should().NotBeNull();
        provider.GetService<ISeriesRepository>().Should().NotBeNull();
        provider.GetService<IMovieRepository>().Should().NotBeNull();
    }

    [Fact]
    public async Task OpenApi_ShouldBeAvailableInDevelopment()
    {
        var response = await _client.GetAsync("/openapi/v1.json");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("/movie");
    }
}
