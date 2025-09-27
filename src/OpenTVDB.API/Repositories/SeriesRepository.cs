using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;
using OpenTVDB.API.Entities;
using OpenTVDB.API.QueryParams;

namespace OpenTVDB.API.Repositories;

public interface ISeriesRepository
{
    Task<List<Series>> Search(SeriesSearchQueryParams queryParams);
    Task<Series?> Get(Guid id);
    Task<Series> Create(Series item);
    Task<Series> Update(Series item);
}

public class SeriesRepository(OpenTVDBContext context) : ISeriesRepository
{
    public Task<List<Series>> Search(SeriesSearchQueryParams queryParams)
    {
        var query = context.Series.AsQueryable();

        if (queryParams.Query != null)
        {
            query = query.Where(x => x.Title.Contains(queryParams.Query));
        }

        return query.ToListAsync();
    }

    public Task<Series?> Get(Guid id)
    {
        return context.Series.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Series> Create(Series item)
    {
        context.Series.Add(item);
        await context.SaveChangesAsync();

        return item;
    }

    public async Task<Series> Update(Series item)
    {
        context.Series.Update(item);
        await context.SaveChangesAsync();

        return item;
    }
}
