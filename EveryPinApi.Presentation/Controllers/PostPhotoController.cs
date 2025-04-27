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
        
        // 기존 DTO를 새 DTO로 변환
        var postPhotoResponses = postPhotos.Select(p => new PostPhotoResponse
        {
            PostPhotoId = p.PostPhotoId,
            PostId = p.PostId, // 속성명 수정
            PhotoUrl = p.photoUrl, // 소문자 p로 시작하는 속성명 사용
            UpdateDate = DateTime.Now, // 기존 DTO에 없는 속성, 임시값 사용
            CreatedDate = DateTime.Now // 기존 DTO에 없는 속성, 임시값 사용
        }).ToList();
        
        return Ok(postPhotoResponses);
    }

    [HttpGet("{postId:int}", Name = "GetPostPhotoById")]
    [ProducesDefaultResponseType(typeof(IEnumerable<PostPhotoResponse>))]
    public async Task<IActionResult> GetPostPhotoToPostId(int postId)
    {
        var postPhotos = await _service.PostPhotoService.GetPostPhotoToPostId(postId, trackChanges: false);

        // 기존 DTO를 새 DTO로 변환
        var postPhotoResponses = postPhotos.Select(p => new PostPhotoResponse
        {
            PostPhotoId = p.PostPhotoId,
            PostId = p.PostId, // 속성명 수정
            PhotoUrl = p.photoUrl, // 소문자 p로 시작하는 속성명 사용
            UpdateDate = DateTime.Now, // 기존 DTO에 없는 속성, 임시값 사용
            CreatedDate = DateTime.Now // 기존 DTO에 없는 속성, 임시값 사용
        }).ToList();

        return Ok(postPhotoResponses);
    }

    [HttpPost]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> CreatePostPhoto([FromBody] PostPhotoResponse postPhotoRequest)
    {
        if (postPhotoRequest is null)
            return BadRequest("게시글 사진 데이터가 빈 값입니다.");

        string UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        // 새 DTO에서 기존 서비스 계층 DTO로 변환
        var createPostPhotoDto = new CreatePostPhotoDto(
            postPhotoRequest.PostId,
            postPhotoRequest.PhotoUrl
        );

        var createPostPhoto = await _service.PostPhotoService.CreatePostPhoto(createPostPhotoDto);

        // 기존 DTO를 새 DTO로 변환
        var postPhotoResponse = new PostPhotoResponse
        {
            PostPhotoId = createPostPhoto.PostPhotoId,
            PostId = createPostPhoto.PostId, // 속성명 수정
            PhotoUrl = createPostPhoto.photoUrl, // 소문자 p로 시작하는 속성명 사용
            UpdateDate = DateTime.Now, // 기존 DTO에 없는 속성, 임시값 사용
            CreatedDate = DateTime.Now // 기존 DTO에 없는 속성, 임시값 사용
        };

        return CreatedAtRoute("GetPostPhotoById", new { postId = postPhotoResponse.PostId }, postPhotoResponse);
    }
}
