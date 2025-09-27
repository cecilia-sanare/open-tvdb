using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Repositories;

public interface IMovieRepository
{
    Task<List<Movie>> Search();
    Task<Movie?> Get(Guid id);
    Task<Movie> Create(Movie item);
    Task<Movie> Update(Movie item);
}

public class MovieRepository(OpenTVDBContext context) : IMovieRepository
{
    public Task<List<Movie>> Search()
    {
        return context.Movies.ToListAsync();
    }

    public Task<Movie?> Get(Guid id)
    {
        return context.Movies.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Movie> Create(Movie item)
    {
        context.Movies.Add(item);
        await context.SaveChangesAsync();

        return item;
    }

    public async Task<Movie> Update(Movie item)
    {
        context.Movies.Update(item);
        await context.SaveChangesAsync();

        return item;
    }
}
