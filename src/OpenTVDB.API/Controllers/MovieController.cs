using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenTVDB.API.Entities;
using OpenTVDB.API.QueryParams;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController(IMovieService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public async Task<ActionResult<List<Movie>>> Search([FromQuery] MovieSearchQueryParams queryParams)
    {
        return await service.Search(queryParams);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Movie>> Get([FromRoute] Guid id)
    {
        var movie = await service.Get(id);

        if (movie == null) return NotFound();

        return movie;
    }

    [HttpPost]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Movie>> Create(Movie media)
    {
        return await service.Create(media);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(200)]
    public async Task<ActionResult<Movie>> Update([FromRoute] Guid id, [FromBody] Movie media)
    {
        if (id != media.Id) return BadRequest("Invalid movie id");

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
