using OpenTVDB.API.Repositories;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Services;

public interface ISeriesService
{
    Task<List<Series>> Search();
    Task<Series?> Get(Guid id);
    Task<Series> Create(Series media);
    Task<Series> Update(Series media);
}

public class SeriesService(ISeriesRepository repository) : ISeriesService
{
    public Task<List<Series>> Search()
    {
        return repository.Search();
    }

    public Task<Series?> Get(Guid id)
    {
        return repository.Get(id);
    }

    public Task<Series> Create(Series media)
    {
        return repository.Create(media);
    }

    public Task<Series> Update(Series media)
    {
        return repository.Update(media);
    }
}
