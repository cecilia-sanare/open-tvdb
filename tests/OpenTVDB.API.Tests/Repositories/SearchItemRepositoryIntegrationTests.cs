using FluentAssertions;
using OpenTVDB.API.Entities;
using OpenTVDB.API.Enums;
using OpenTVDB.API.Repositories;

namespace OpenTVDB.API.Tests.Repositories;

public class SearchItemRepositoryIntegrationTests : DatabaseTest
{
    private readonly SearchItemRepository _repository;

    public SearchItemRepositoryIntegrationTests(WebApplicationFactoryTest factory) : base(factory)
    {
        _repository = new SearchItemRepository(Context);
    }

    #region Search

    // Not a fan of this since it doesn't really validate the view :<
    [Fact]
    public async Task Search_ShouldSupportRetrievingSearchItems()
    {
        var searchItems = DataFactory.Many(DataFactory.SearchItem, 10);
        Context.SearchItems.AddRange(searchItems);
        await Context.SaveChangesAsync();

        var items = await _repository.Search();

        items.Count.Should().Be(searchItems.Count);
        items.Should().BeEquivalentTo(searchItems);
    }

    #endregion
}
