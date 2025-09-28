using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using OpenTVDB.API.Entities;
using OpenTVDB.API.Services;

namespace OpenTVDB.API.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class SearchController(ISearchItemService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    [Description]
    public async Task<ActionResult<List<SearchItem>>> Search()
    {
        return await service.Search();
    }
}
