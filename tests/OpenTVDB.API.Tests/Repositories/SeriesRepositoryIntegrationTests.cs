using FluentAssertions;
using OpenTVDB.API.Repositories;

namespace OpenTVDB.API.Tests.Repositories;

public class SeriesRepositoryIntegrationTests : DBTest
{
    private readonly SeriesRepository _repository;
    
    public SeriesRepositoryIntegrationTests()
    {
        _repository = new SeriesRepository(Context);
    }

    #region Search
    
    [Fact]
    public async Task Search_ShouldReturnTheSeries()
    {
        var expectedSeries = DataFactory.Many(DataFactory.Series, 10);
        Context.Series.AddRange(expectedSeries);
        await Context.SaveChangesAsync();
        
        var series = await _repository.Search();

        series.Count.Should().Be(expectedSeries.Count);
        series.Should().BeEquivalentTo(expectedSeries);
    }

    #endregion
    
    #region Create
    
    [Fact]
    public async Task Create_ShouldCreateTheSeries()
    {
        var expectedSeries = DataFactory.Series();
        
        var series = await _repository.Create(expectedSeries);

        series.Id.Should().Be(expectedSeries.Id);
        series.Created.Should().BeAfter(TestStart);
        series.Updated.Should().Be(null);
    }
    
    #endregion
    
    #region Update
    
    [Fact]
    public async Task Update_ShouldCreateTheSeries()
    {
        var expectedSeries = DataFactory.Series();
        Context.Series.AddRange(expectedSeries);
        await Context.SaveChangesAsync();
        
        var series = await _repository.Update(expectedSeries);

        series.Updated.Should().BeAfter(series.Created);
    }
    
    #endregion
}