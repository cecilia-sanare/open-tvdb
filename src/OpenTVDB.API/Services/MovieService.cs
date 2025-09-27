using OpenTVDB.API.Repositories;
using OpenTVDB.API.Entities;
using OpenTVDB.API.QueryParams;

namespace OpenTVDB.API.Services;

public interface IMovieService
{
    Task<List<Movie>> Search(MovieSearchQueryParams queryParams);
    Task<Movie?> Get(Guid id);
    Task<Movie> Create(Movie media);
    Task<Movie> Update(Movie media);
}

public class MovieService(IMovieRepository repository) : IMovieService
{
    public Task<List<Movie>> Search(MovieSearchQueryParams queryParams)
    {
        return repository.Search(queryParams);
    }

    public Task<Movie?> Get(Guid id)
    {
        return repository.Get(id);
    }

    public Task<Movie> Create(Movie media)
    {
        return repository.Create(media);
    }

    public Task<Movie> Update(Movie media)
    {
        return repository.Update(media);
    }
}
