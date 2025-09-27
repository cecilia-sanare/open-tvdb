using FluentAssertions;
using Moq;
using OpenTVDB.API.Entities;
using OpenTVDB.API.QueryParams;
using OpenTVDB.API.Repositories;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Tests.Services;

public class SeriesServiceTest
{
    private readonly Mock<ISeriesRepository> _repository = new();
    private readonly SeriesService _service;

    public SeriesServiceTest()
    {
        _service = new SeriesService(_repository.Object);
    }

    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheSeries()
    {
        var queryParams = new SeriesSearchQueryParams();
        var expectedSeries = DataFactory.Many(DataFactory.Series, 10);
        _repository.Setup(x => x.Search(queryParams)).ReturnsAsync(expectedSeries);

        var series = await _service.Search(queryParams);

        series.Should().BeEquivalentTo(expectedSeries);
    }

    #endregion

    #region Get

    [Fact]
    public async Task Get_ShouldReturnTheSeries()
    {
        var expectedSeries = DataFactory.Series(x => x.Id = Guid.NewGuid());
        _repository.Setup(x => x.Get(expectedSeries.Id!.Value)).ReturnsAsync(expectedSeries);

        var series = await _service.Get(expectedSeries.Id!.Value);

        series.Should().BeEquivalentTo(expectedSeries);
    }

    [Fact]
    public async Task Get_ShouldReturnNull_WhenTheSeriesDoesNotExist()
    {
        var expectedSeries = DataFactory.Series(x => x.Id = Guid.NewGuid());
        _repository.Setup(x => x.Get(expectedSeries.Id!.Value)).ReturnsAsync((Series?)null);

        var series = await _service.Get(expectedSeries.Id!.Value);

        series.Should().BeNull();
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ShouldReturnTheSeries()
    {
        var expectedSeries = DataFactory.Series();
        _repository.Setup(x => x.Create(expectedSeries)).ReturnsAsync(expectedSeries);

        var series = await _service.Create(expectedSeries);

        series.Should().BeEquivalentTo(expectedSeries);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_ShouldReturnTheSeries()
    {
        var expectedSeries = DataFactory.Series();
        _repository.Setup(x => x.Update(expectedSeries)).ReturnsAsync(expectedSeries);

        var series = await _service.Update(expectedSeries);

        series.Should().BeEquivalentTo(expectedSeries);
    }

    #endregion
}
