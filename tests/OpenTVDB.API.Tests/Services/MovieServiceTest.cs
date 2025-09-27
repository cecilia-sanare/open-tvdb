using FluentAssertions;
using Moq;
using OpenTVDB.API.Entities;
using OpenTVDB.API.QueryParams;
using OpenTVDB.API.Repositories;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Tests.Services;

public class MovieServiceTest
{
    private readonly Mock<IMovieRepository> _repository = new();
    private readonly MovieService _service;

    public MovieServiceTest()
    {
        _service = new MovieService(_repository.Object);
    }

    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheMovie()
    {
        var queryParams = new MovieSearchQueryParams();
        var expectedMovie = DataFactory.Many(DataFactory.Movie, 10);
        _repository.Setup(x => x.Search(queryParams)).ReturnsAsync(expectedMovie);

        var movie = await _service.Search(queryParams);

        movie.Should().BeEquivalentTo(expectedMovie);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnTheMovie()
    {
        var expectedMovie = DataFactory.Movie(x => x.Id = Guid.NewGuid());
        _repository.Setup(x => x.Get(expectedMovie.Id!.Value)).ReturnsAsync(expectedMovie);

        var movie = await _service.Get(expectedMovie.Id!.Value);

        movie.Should().BeEquivalentTo(expectedMovie);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenTheMovieDoesNotExist()
    {
        var expectedMovie = DataFactory.Movie(x => x.Id = Guid.NewGuid());
        _repository.Setup(x => x.Get(expectedMovie.Id!.Value)).ReturnsAsync((Movie?)null);

        var movie = await _service.Get(expectedMovie.Id!.Value);

        movie.Should().BeNull();
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ShouldReturnTheMovie()
    {
        var expectedMovie = DataFactory.Movie();
        _repository.Setup(x => x.Create(expectedMovie)).ReturnsAsync(expectedMovie);

        var movie = await _service.Create(expectedMovie);

        movie.Should().BeEquivalentTo(expectedMovie);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_ShouldReturnTheMovie()
    {
        var expectedMovie = DataFactory.Movie();
        _repository.Setup(x => x.Update(expectedMovie)).ReturnsAsync(expectedMovie);

        var movie = await _service.Update(expectedMovie);

        movie.Should().BeEquivalentTo(expectedMovie);
    }

    #endregion
}
