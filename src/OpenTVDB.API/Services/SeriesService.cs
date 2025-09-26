using OpenTVDB.API.Repositories;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Services;

public interface ISeriesService
{
    Task<List<Series>> Search();
    Task<Series> Create(Series media);
    Task<Series> Update(Series media);
}

public class SeriesService(ISeriesRepository itemRepository) : ISeriesService
{
    public Task<List<Series>> Search()
    {
        return itemRepository.Search();
    }
    
    public Task<Series> Create(Series media)
    {
        return itemRepository.Create(media);
    }
    
    public Task<Series> Update(Series media)
    {
        return itemRepository.Update(media);
    }
}