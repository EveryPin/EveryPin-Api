using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveryPinApi.Presentation.Controllers;

[Route("api/map")]
[ApiController]
public class MapController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IServiceManager _service;

    public MapController(ILogger<MapController> logger, IServiceManager service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("pin")]
    public async Task<IActionResult> GetSearchPost([FromQuery] double x,
                                                   [FromQuery] double y, 
                                                   [FromQuery] double range)
    {
        var posts = await _service.PostService.GetSearchPost(x, y, range, trackChanges: false);
        return Ok(posts);
    }

    [HttpGet("pin/{userId}", Name = "GetSearchUserPost")]
    public async Task<IActionResult> GetSearchUserPost(string userId)
    {
        //var posts = await _service.PostService.GetSearchPost(userId, trackChanges: false);
        //return Ok(posts);
        return StatusCode(501);
    }
}
