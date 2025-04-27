using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.Dtos.Map.Requests;
using Shared.Dtos.Post.Responses;

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
    [ProducesDefaultResponseType(typeof(IEnumerable<PostDetailResponse>))]
    public async Task<IActionResult> GetSearchPost([FromQuery] SearchPostRequest searchRequest)
    {
        var posts = await _service.PostService.GetSearchPost(searchRequest.x, searchRequest.y, searchRequest.range, trackChanges: false);
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
