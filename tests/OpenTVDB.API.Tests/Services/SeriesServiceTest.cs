using FluentAssertions;
using Moq;
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
        var expectedSeries = DataFactory.Many(DataFactory.Series, 10);
        _repository.Setup(x => x.Search()).ReturnsAsync(expectedSeries);
        
        var series = await _service.Search();

        series.Should().BeEquivalentTo(expectedSeries);
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