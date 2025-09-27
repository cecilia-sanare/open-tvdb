using FluentAssertions;
using OpenTVDB.API.Repositories;

namespace OpenTVDB.API.Tests.Repositories;

public class MovieRepositoryIntegrationTests : DBTest
{
    private readonly MovieRepository _repository;

    public MovieRepositoryIntegrationTests()
    {
        _repository = new MovieRepository(Context);
    }

    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheMovie()
    {
        var expectedMovie = DataFactory.Many(DataFactory.Movie, 10);
        Context.Movies.AddRange(expectedMovie);
        await Context.SaveChangesAsync();

        var movie = await _repository.Search();

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

        var movie = await _repository.Get(expectedMovie.Id!.Value);

        movie.Should().BeEquivalentTo(expectedMovie);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenTheMovieDoesNotExist()
    {
        var movie = await _repository.Get(Guid.NewGuid());

        movie.Should().BeNull();
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ShouldCreateTheMovie()
    {
        var expectedMovie = DataFactory.Movie();

        var movie = await _repository.Create(expectedMovie);

        movie.Id.Should().Be(expectedMovie.Id);
        movie.Created.Should().BeAfter(TestStart);
        movie.Updated.Should().Be(null);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_ShouldCreateTheMovie()
    {
        var expectedMovie = DataFactory.Movie();
        Context.Movies.AddRange(expectedMovie);
        await Context.SaveChangesAsync();

        var movie = await _repository.Update(expectedMovie);

        movie.Updated.Should().BeAfter(movie.Created);
    }

    #endregion
}
