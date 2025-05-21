using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.Dtos.Post.Requests;
using Shared.Dtos.Post.Responses;
using Shared.Dtos.Comment.Requests;
using Shared.Dtos.Comment.Responses;
using Shared.Dtos.Like.Responses;
using Shared.Dtos.Like.Requests;
using System.Security.Claims;

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
    [ProducesDefaultResponseType(typeof(PostDetailResponse))]
    public async Task<IActionResult> GetPost(int postId)
    {
        var post = await _service.PostService.GetPost(postId, trackChanges: false);
        return Ok(post);
    }

    /// <summary>
    /// 게시글 작성
    /// </summary>
    /// <param name="createPostRequest"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> CreatePost([FromForm] CreatePostRequest createPostRequest)
    {
        if (createPostRequest is null)
            return BadRequest("게시글의 내용이 비었습니다.");
            
        // 로그인 유저 ID
        string userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createdPost = await _service.PostService.CreatePost(createPostRequest, userId);
        return CreatedAtRoute("GetPostById", new { postId = createdPost.PostId }, createdPost);
    }

    /// <summary>
    /// 게시글 수정
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="updatePostRequest"></param>
    /// <returns></returns>
    [HttpPatch("{postId:int}", Name = "UpdatePost")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> UpdatePost(int postId,
                                               [FromBody] UpdatePostRequest updatePostRequest)
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

        string userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var createLikeRequest = new CreateLikeRequest { PostId = postId };
        var createdLike = await _service.LikeService.CreateLike(userId, createLikeRequest);
        
        return CreatedAtRoute("GetLikeToPostId", new { postId = createdLike.PostId }, createdLike);
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
    [ProducesDefaultResponseType(typeof(IEnumerable<CommentResponse>))]
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
    /// <param name="createCommentRequest"></param>
    /// <returns></returns>
    [HttpPost("{postId:int}/comment", Name = "CreateComment")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> CreateComment(int postId,
                                                   [FromBody] CreateCommentRequest createCommentRequest)
    {
        if (string.IsNullOrEmpty(createCommentRequest.CommentMessage))
            return BadRequest("댓글 내용이 작성되지 않았습니다.");
        else if (postId <= 0)
            return BadRequest("PostId 값이 비정상입니다.");
    
        string userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        // PostId 설정
        createCommentRequest.PostId = postId;

        var createdComment = await _service.CommentService.CreateComment(userId, createCommentRequest);
        return CreatedAtRoute("GetCommentToPostId", new { postId = createdComment.PostId }, createdComment);
    }

    /// <summary>
    /// 댓글 수정
    /// </summary>
    /// <param name="postId"></param>
    /// <param name="commentId"></param>
    /// <param name="updateCommentRequest"></param>
    /// <returns></returns>
    [HttpPatch("{postId:int}/comment/{commentId:int}", Name = "UpdateComment")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> UpdateComment(int postId,
                                                   int commentId,
                                                   [FromBody] UpdateCommentRequest updateCommentRequest)
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
