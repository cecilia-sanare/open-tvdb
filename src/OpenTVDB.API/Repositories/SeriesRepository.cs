using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Repositories;

public interface ISeriesRepository
{
    Task<List<Series>> Search();
    Task<Series> Create(Series item);
    Task<Series> Update(Series item);
}

public class SeriesRepository(OpenTVDBContext context) : ISeriesRepository
{
    public Task<List<Series>> Search()
    {
        return context.Series.ToListAsync();
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