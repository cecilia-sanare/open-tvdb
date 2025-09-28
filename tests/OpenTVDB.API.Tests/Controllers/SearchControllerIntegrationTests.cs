using System.Net.Http.Json;
using FluentAssertions;
using OpenTVDB.API.Entities;
using OpenTVDB.API.Repositories;

namespace OpenTVDB.API.Tests.Controllers;

public class SearchControllerIntegrationTests(WebApplicationFactoryTest factory) : DatabaseTest(factory)
{
    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheSearchItems()
    {
        var expectedSearchItems = DataFactory.Many(DataFactory.SearchItem, 10);
        Context.SearchItems.AddRange(expectedSearchItems);
        await Context.SaveChangesAsync();

        var client = Factory.CreateClient();

        var searchItems = await client.GetFromJsonAsync<List<SearchItem>>("/api/search");

        searchItems.Should().NotBeNull();
        searchItems.Count.Should().Be(expectedSearchItems.Count);
        searchItems.Should().BeEquivalentTo(expectedSearchItems);
    }

    #endregion
}
