using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Database;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Repositories;

public interface ISearchItemRepository
{
    Task<List<SearchItem>> Search();
}

public class SearchItemRepository(OpenTVDBContext context) : ISearchItemRepository
{
    public Task<List<SearchItem>> Search()
    {
        return context.SearchItems.ToListAsync();
    }
}
