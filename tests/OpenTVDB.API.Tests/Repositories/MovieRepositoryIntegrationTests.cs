using FluentAssertions;
using OpenTVDB.API.QueryParams;
using OpenTVDB.API.Repositories;

namespace OpenTVDB.API.Tests.Repositories;

public class MovieRepositoryIntegrationTests : DatabaseTest
{
    private readonly MovieRepository _repository;

    public MovieRepositoryIntegrationTests(WebApplicationFactoryTest factory) : base(factory)
    {
        _repository = new MovieRepository(Context);
    }

    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheMovie()
    {
        var queryParams = new MovieSearchQueryParams();
        var expectedMovies = DataFactory.Many(DataFactory.Movie, 10);
        Context.Movies.AddRange(expectedMovies);
        await Context.SaveChangesAsync();

        var movies = await _repository.Search(queryParams);

        movies.Count.Should().Be(expectedMovies.Count);
        movies.Should().BeEquivalentTo(expectedMovies);
    }

    [Fact]
    public async Task Search_ShouldReturnMoviesContainingTheGivenName_WhenTheQueryIsProvided()
    {
        var queryParams = new MovieSearchQueryParams()
        {
            Query = "Hunter"
        };
        var expectedMovie = DataFactory.Movie(x => x.Title = "Hunter x Hunter");
        var unexpectedMovies = DataFactory.Many(DataFactory.Movie, 10);
        Context.Movies.AddRange(expectedMovie);
        Context.Movies.AddRange(unexpectedMovies);
        await Context.SaveChangesAsync();

        var movies = await _repository.Search(queryParams);

        movies.Count.Should().Be(1);
        movies.Should().BeEquivalentTo([expectedMovie]);
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
