using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using OpenTVDB.API.Entities;

namespace OpenTVDB.API.Tests.Controllers;

public class SeriesControllerIntegrationTests(WebApplicationFactoryTest factory) : ControllerTest(factory)
{
    #region Search

    [Fact]
    public async Task Search_ShouldReturnTheSeries()
    {
        var expectedSeries = DataFactory.Many(DataFactory.Series, 10);
        Context.Series.AddRange(expectedSeries);
        await Context.SaveChangesAsync();

        var client = factory.CreateClient();

        var response = await client.GetAsync("/Series");
        var series = await response.Content.ReadFromJsonAsync<List<Series>>();

        series.Should().NotBeNull();
        series.Count.Should().Be(expectedSeries.Count);
        series.Should().BeEquivalentTo(expectedSeries);
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_ShouldCreateTheSeries()
    {
        var expectedSeries = DataFactory.Series();

        var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/Series", expectedSeries);
        var series = await response.Content.ReadFromJsonAsync<Series>();

        series.Should().NotBeNull();
        series.Id.Should().NotBeNull();
        series.Created.Should().BeAfter(TestStart);
        series.Should().BeEquivalentTo(expectedSeries, options => options.Excluding(x => x.Id).Excluding(x => x.Created));
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_ShouldUpdateTheSeries()
    {
      var expectedSeries = DataFactory.Series();
      Context.Series.AddRange(expectedSeries);
      await Context.SaveChangesAsync();

      var client = factory.CreateClient();

      var response = await client.PutAsJsonAsync($"/Series/{expectedSeries.Id}", expectedSeries);
      var series = await response.Content.ReadFromJsonAsync<Series>();

      series.Should().NotBeNull();
      series.Updated.Should().BeAfter(series.Created);
      series.Should().BeEquivalentTo(expectedSeries, options => options.Excluding(x => x.Updated));
    }

    [Fact]
    public async Task Update_ShouldReturnBadRequest_WhenIdIsInvalid()
    {
      var client = factory.CreateClient();

      var response = await client.PutAsJsonAsync($"/Series/{Guid.NewGuid()}", DataFactory.Series());

      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

      var message = await response.Content.ReadAsStringAsync();
      message.Should().Contain("Invalid series id");
    }

    #endregion
}
