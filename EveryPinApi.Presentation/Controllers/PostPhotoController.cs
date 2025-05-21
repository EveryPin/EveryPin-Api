using Entites.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.Dtos.Post.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EveryPinApi.Presentation.Controllers;

[Route("api/postphoto")]
[ApiController]
public class PostPhotoController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IServiceManager _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostPhotoController(ILogger<PostPhotoController> logger, IServiceManager service, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    [Authorize(Roles = "NormalUser")]
    [ProducesDefaultResponseType(typeof(IEnumerable<PostPhotoResponse>))]
    public async Task<IActionResult> GetAllPostPhoto()
    {
        var postPhotos = await _service.PostPhotoService.GetAllPostPhoto(trackChanges: false);

        var postPhotoResponses = postPhotos.Select(p => new PostPhotoResponse
        {
            PostPhotoId = p.PostPhotoId,
            PostId = p.PostId,
            PhotoUrl = p.PhotoUrl,
            UpdateDate = p.UpdateDate,
            CreatedDate = p.CreatedDate,
        }).ToList();
        
        return Ok(postPhotoResponses);
    }

    [HttpGet("{postId:int}", Name = "GetPostPhotoById")]
    [ProducesDefaultResponseType(typeof(IEnumerable<PostPhotoResponse>))]
    public async Task<IActionResult> GetPostPhotoToPostId(int postId)
    {
        var postPhotos = await _service.PostPhotoService.GetPostPhotoToPostId(postId, trackChanges: false);
        
        var postPhotoResponses = postPhotos.Select(p => new PostPhotoResponse
        {
            PostPhotoId = p.PostPhotoId,
            PostId = p.PostId,
            PhotoUrl = p.PhotoUrl,
            UpdateDate = p.UpdateDate,
            CreatedDate = p.CreatedDate,
        }).ToList();

        return Ok(postPhotoResponses);
    }
}
