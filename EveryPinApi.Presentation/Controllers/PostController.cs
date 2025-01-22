using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DataTransferObject;
using Shared.DataTransferObject.InputDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EveryPinApi.Presentation.Controllers;

[Route("api/post")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IServiceManager _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostController(ILogger<PostController> logger, IServiceManager service, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    #region 게시글
    /// <summary>
    /// 게시글 조회
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    [HttpGet("{postId:int}", Name = "GetPostById")]
    [ProducesDefaultResponseType(typeof(PostDto))]
    public async Task<IActionResult> GetPost(int postId)
    {
        var post = await _service.PostService.GetPost(postId, trackChanges: false);

        return Ok(post);
    }

    /// <summary>
    /// 게시글 작성
    /// </summary>
    /// <param name="inputPost"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostInputDto inputPost)
    {
        CreatePostDto post = new();
        post.SetInputDto(inputPost);

        if (post is null)
            return BadRequest("게시글의 내용이 비었습니다.");

        // 로그인 유저 ID로 생성하도록 처리
        post.UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createPost = await _service.PostService.CreatePost(post);

        return CreatedAtRoute("GetPostById", new { postId = createPost.PostId }, createPost);
    }

    /// <summary>
    /// 게시글 수정
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="inputPost"></param>
    /// <returns></returns>
    [HttpPut("{postId:int}", Name = "UpdatePost")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> UpdatePost(int postId,
                                               [FromBody] CreatePostInputDto inputPost)
    {
        return StatusCode(501);
    }

    /// <summary>
    /// 게시글 삭제
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    [HttpDelete("{postId:int}", Name = "DeletePost")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        return StatusCode(501);
    }
    #endregion

    #region 좋아요
    /// <summary>
    /// 게시글 좋아요 추가
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    [HttpPost("{postId:int}/like", Name = "LikePost")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> LikePost(int postId)
    {
        if (postId <= 0)
            return BadRequest("postId 값이 비정상입니다.");

        string UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        CreateLikeDto like = new CreateLikeDto(postId, UserId);

        var createLike = await _service.LikeService.CreateLike(like);

        return CreatedAtRoute("GetLikeToPostId", new { postId = createLike.PostId }, createLike);
    }

    /// <summary>
    /// 게시글 좋아요 취소
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    [HttpDelete("{postId:int}/like", Name = "UnLikePost")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> UnLikePost(int postId)
    {
        return StatusCode(501);
    }
    #endregion

    #region 댓글
    /// <summary>
    /// 댓글 조회 (페이징)
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    [HttpGet("{postId:int}/comment", Name = "GetCommentToPostId")]
    [ProducesDefaultResponseType(typeof(CommentDto))]
    public async Task<IActionResult> GetCommentToPostId(int postId,
                                                       [FromQuery] int page,
                                                       [FromQuery] int size)
    {
        var comments = await _service.CommentService.GetCommentToPostId(postId, trackChanges: false);
    
        return Ok(comments);
    }

    /// <summary>
    /// 댓글 작성
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="commentMessage"></param>
    /// <returns></returns>
    [HttpPost("{postId:int}/comment", Name = "CreateComment")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> CreateComment(int postId,
                                                   [FromBody] string commentMessage)
    {
        if (string.IsNullOrEmpty(commentMessage))
            return BadRequest("댓글 내용이 작성되지 않았습니다.");
        else if (postId <= 0)
            return BadRequest("PostId 값이 비정상입니다.");
    
        string UserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    
        CreateCommentDto comment = new CreateCommentDto(postId, UserId, commentMessage);
    
        var createComment = await _service.CommentService.CreateComment(comment);
    
        return CreatedAtRoute("GetCommentToPostId", new { postId = createComment.PostId }, createComment);
    }

    /// <summary>
    /// 댓글 수정
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="commentId"></param>
    /// <param name="commentMessage"></param>
    /// <returns></returns>
    [HttpPut("{postId:int}/comment/{commentId:int}", Name = "UpdateComment")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> UpdateComment(int postId,
                                                   int commentId,
                                                   [FromBody] string commentMessage)
    {
        return StatusCode(501);
    }

    /// <summary>
    /// 댓글 삭제
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="commentId"></param>
    /// <returns></returns>
    [HttpDelete("{postId:int}/comment/{commentId:int}", Name = "DeleteComment")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> DeleteComment(int postId, int commentId)
    {
        return StatusCode(501);
    }
    #endregion
}
