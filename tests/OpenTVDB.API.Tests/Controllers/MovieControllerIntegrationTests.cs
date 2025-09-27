using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Tests.Controllers;

public class MovieControllerIntegrationTests(WebApplicationFactoryTest factory) : ControllerTest(factory)
{
    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheMovie()
    {
        var expectedMovie = DataFactory.Many(DataFactory.Movie, 10);
        Context.Movies.AddRange(expectedMovie);
        await Context.SaveChangesAsync();

        var client = Factory.CreateClient();

        var response = await client.GetAsync("/Movie");
        var movie = await response.Content.ReadFromJsonAsync<List<Movie>>();

        movie.Should().NotBeNull();
        movie.Count.Should().Be(expectedMovie.Count);
        movie.Should().BeEquivalentTo(expectedMovie);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnTheMovie()
    {
        var expectedMovie = DataFactory.Movie();
        Context.Movies.AddRange(expectedMovie);
        await Context.SaveChangesAsync();

        var client = Factory.CreateClient();

        var response = await client.GetAsync($"/Movie/{expectedMovie.Id}");
        var movie = await response.Content.ReadFromJsonAsync<Movie>();

        movie.Should().BeEquivalentTo(expectedMovie);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenTheMovieDoesNotExist()
    {
        var client = Factory.CreateClient();

        var response = await client.GetAsync($"/Movie/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ShouldCreateTheMovie()
    {
        var expectedMovie = DataFactory.Movie();

        var client = Factory.CreateClient();

        var response = await client.PostAsJsonAsync("/Movie", expectedMovie);
        var movie = await response.Content.ReadFromJsonAsync<Movie>();

        movie.Should().NotBeNull();
        movie.Id.Should().NotBeNull();
        movie.Created.Should().BeAfter(TestStart);
        movie.Should().BeEquivalentTo(expectedMovie, options => options.Excluding(x => x.Id).Excluding(x => x.Created));
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_ShouldUpdateTheMovie()
    {
        var expectedMovie = DataFactory.Movie();
        Context.Movies.AddRange(expectedMovie);
        await Context.SaveChangesAsync();

        var client = Factory.CreateClient();

        var response = await client.PutAsJsonAsync($"/Movie/{expectedMovie.Id}", expectedMovie);
        var movie = await response.Content.ReadFromJsonAsync<Movie>();

        movie.Should().NotBeNull();
        movie.Updated.Should().BeAfter(movie.Created);
        movie.Should().BeEquivalentTo(expectedMovie, options => options.Excluding(x => x.Updated));
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenTheMovieDoesNotExist()
    {
        var expectedMovie = DataFactory.Movie(x => x.Id = Guid.NewGuid());

        var client = Factory.CreateClient();

        var response = await client.PutAsJsonAsync($"/Movie/{expectedMovie.Id}", expectedMovie);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
        var client = Factory.CreateClient();

        var response = await client.PutAsJsonAsync($"/Movie/{Guid.NewGuid()}", DataFactory.Movie());

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var message = await response.Content.ReadAsStringAsync();
        message.Should().Contain("Invalid movie id");
    }

    #endregion
}
