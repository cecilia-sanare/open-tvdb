using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Entities;
using OpenTVDB.API.QueryParams;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Controllers;

[ApiController]
[Route("[controller]")]
public class SeriesController(ISeriesService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<Series>>> Search([FromQuery] SeriesSearchQueryParams queryParams)
    {
        return await service.Search(queryParams);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Series>> Get([FromRoute] Guid id)
    {
        var movie = await service.Get(id);

        if (movie == null) return NotFound();

        return movie;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Series>> Create(Series media)
    {
        return await service.Create(media);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Series>> Update([FromRoute] Guid id, [FromBody] Series media)
    {
        if (id != media.Id) return BadRequest("Invalid series id");

        try
        {
            return await service.Update(media);
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }
    }
}
