using OpenTVDB.API.Entities;
using OpenTVDB.API.Repositories;

namespace OpenTVDB.API.Services;

public interface ISearchItemService
{
    Task<List<SearchItem>> Search();
}

public class SearchItemService(ISearchItemRepository repository) : ISearchItemService
{
    public Task<List<SearchItem>> Search()
    {
        return repository.Search();
    }
}
