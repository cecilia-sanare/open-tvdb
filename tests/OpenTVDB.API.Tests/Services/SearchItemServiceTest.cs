using FluentAssertions;
using Moq;
using OpenTVDB.API.Repositories;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Tests.Services;

public class SearchItemServiceTest
{
    private readonly Mock<ISearchItemRepository> _repository = new();
    private readonly SearchItemService _service;

    public SearchItemServiceTest()
    {
        _service = new SearchItemService(_repository.Object);
    }

    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheSearchItems()
    {
        var expectedSearchItems = DataFactory.Many(DataFactory.SearchItem, 10);
        _repository.Setup(x => x.Search()).ReturnsAsync(expectedSearchItems);

        var series = await _service.Search();

        series.Should().BeEquivalentTo(expectedSearchItems);
    }

    #endregion
}
