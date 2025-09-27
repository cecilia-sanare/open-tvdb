using Microsoft.AspNetCore.Mvc;
using OpenTVDB.API.Entities;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SeriesController(ISeriesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public Task<List<Series>> Search()
    {
        return service.Search();
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public Task<Series> Create(Series media)
    {
        return service.Create(media);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Series>> Update([FromRoute] Guid id, [FromBody] Series media)
    {
        if (id != media.Id) return BadRequest("Invalid series id");

        return await service.Update(media);
    }
}
