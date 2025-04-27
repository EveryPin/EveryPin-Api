using Azure.Core;
using Entites.Models;
using ExternalLibraryService;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.Dtos.Auth.Requests;
using Shared.Dtos.Auth.Responses;
using Shared.Dtos.User.Requests;
using Shared.Dtos.Post.Responses;
using Shared.Dtos.Profile.Responses;
using Shared.Dtos.Like.Responses;
using Shared.Dtos.Common.Blob.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Shared.Dtos.Common;

namespace EveryPinApi.Presentation.Controllers;

[Route("api/test")]
[ApiController]
public class TestApiController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IServiceManager _service;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly BlobHandlingService _blobHandlingService;
    private readonly FirebaseAdminSDKService _firebaseAdminSDKService;

    public TestApiController(ILogger<TestApiController> logger, IServiceManager service, IHttpContextAccessor httpContextAccessor, BlobHandlingService blobHandlingService, FirebaseAdminSDKService firebaseAdminSDKService)
    {
        _logger = logger;
        _service = service;
        _blobHandlingService = blobHandlingService;
        _firebaseAdminSDKService = firebaseAdminSDKService;
        _httpContextAccessor = httpContextAccessor;
    }

    #region 로그인 테스트
    //[HttpPost("auth/regist")]
    ////[ServiceFilter(typeof(ValidationFilterAttribute))]        
    //public async Task<IActionResult> RegisterUser([FromBody] RegisterUserRequest registerUserRequest)
    //{
    //    var result = await _service.AuthenticationService.RegisterUser(new User
    //    {
    //        Name = registerUserRequest.Name,
    //        UserName = registerUserRequest.UserName,
    //        Password = registerUserRequest.Password,
    //        Email = registerUserRequest.Email,
    //        PhoneNumber = registerUserRequest.PhoneNumber,
    //        PlatformCode = registerUserRequest.PlatformCode,
    //        FcmToken = registerUserRequest.FcmToken,
    //        Roles = registerUserRequest.Roles
    //    });
    //
    //    if (!result.Succeeded)
    //    {
    //        foreach (var error in result.Errors)
    //        {
    //            ModelState.TryAddModelError(error.Code, error.Description);
    //        }
    //        return BadRequest(ModelState);
    //    }
    //
    //    var userAccountInfo = await _service.UserService.GetUserByEmail(registerUserRequest.Email, false);
    //
    //    var profile = new Entites.Models.Profile()
    //    {
    //        UserId = userAccountInfo.Id,
    //        ProfileDisplayId = registerUserRequest.Email.Split('@')[0],
    //        ProfileName = registerUserRequest.UserName,
    //        SelfIntroduction = null,
    //        PhotoUrl = null,
    //        User = userAccountInfo,
    //        CreatedDate = DateTime.Now
    //    };
    //
    //    var createdProfile = await _service.ProfileService.CreateProfile(profile);
    //
    //    if (createdProfile != null)
    //    {
    //        return StatusCode(201);
    //    }
    //    else
    //    {
    //        return BadRequest("createdProfile가 null입니다.");
    //    }
    //}

    [HttpPost("auth/login")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest loginRequest)
    {
        var userInfo = await _service.SingleSignOnService.GetUserInfo(loginRequest.PlatformCode, loginRequest.AccessToken);

        if (!await _service.AuthenticationService.ValidateUser(userInfo.UserEmail))
            return Unauthorized();

        var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);
        
        return Ok(tokenDto);
    }

    [HttpGet("auth/kakao-web-login")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> KakaoWebLogin(string code)
    {
        string platformCode = "kakao";
        string kakaoAccessToken = await _service.SingleSignOnService.GetKakaoAccessToken(code);

        // 액세스 토큰을 이용하여 플랫폼에서 유저 정보 받아오기
        var userInfo = await _service.SingleSignOnService.GetUserInfo(platformCode, kakaoAccessToken);
        if (userInfo == null) throw new UnauthorizedAccessException("SSO 인증 실패");

        // 사용자 검증 및 회원가입
        bool isUserExist = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        if (!isUserExist)
        {
            string fcmToken = ""; // 웹 테스트 용도라서 빈문자열로
            var user = await _service.UserService.RegistNewUser(userInfo, fcmToken);
            await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        }

        var tokenDto = await _service.AuthenticationService.CreateTokenWithUpdateFcmToken("", populateExp: true);
        
        return Ok(tokenDto);
    }

    [HttpGet("auth/google-web-login")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> GoogleWebLogin(string code)
    {
        string platformCode = "google";
        string googleAccessToken = await _service.SingleSignOnService.GetGoogleAccessToken(code);

        // 액세스 토큰을 이용하여 플랫폼에서 유저 정보 받아오기
        var userInfo = await _service.SingleSignOnService.GetUserInfo(platformCode, googleAccessToken);
        if (userInfo == null) throw new UnauthorizedAccessException("SSO 인증 실패");

        // 사용자 검증 및 회원가입
        bool isUserExist = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        if (!isUserExist)
        {
            string fcmToken = ""; // 웹 테스트 용도라서 빈문자열로
            var user = await _service.UserService.RegistNewUser(userInfo, fcmToken);
            await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        }

        var tokenDto = await _service.AuthenticationService.CreateTokenWithUpdateFcmToken("", populateExp: true);
        
        return Ok(tokenDto);
    }
    #endregion

    #region Blob Storage 테스트
    [HttpGet("blob/listup-blob")]
    public async Task<IActionResult> TestGetAllBlob()
    {
        var result = await _blobHandlingService.ListAsync();
        
        var blobResponses = result.Select(r => new BlobResponse
        {
            IsSuccess = r.Uri != null,
            Message = r.Uri != null ? "Blob 조회 성공" : "Blob 조회 실패",
            FileUrl = r.Uri
        }).ToList();
        
        return Ok(blobResponses);
    }

    [HttpPost("blob/upload-blob")]
    public async Task<IActionResult> TestUploadToBlobStorage(IFormFile file)
    {
        var result = await _blobHandlingService.UploadAsync(file);

        var blobResponse = new BlobResponse
        {
            IsSuccess = !result.Error,
            Message = result.Message,
            FileUrl = result.Blob?.Uri
        };

        if (result.Error)
            return StatusCode(415, blobResponse);
        else
            return Ok(blobResponse);
    }

    [HttpGet("blob/download-blob")]
    public async Task<IActionResult> TestDownloadToBlobStorage(string fileName)
    {
        var result = await _blobHandlingService.DownloadAsync(fileName);
        return File(result.Content, result.ContentType, result.Name);
    }

    [HttpDelete("blob/delete-blob")]
    public async Task<IActionResult> TestDeleteToBlobStorage(string fileName)
    {
        var result = await _blobHandlingService.DeleteAsync(fileName);
        
        // 기존 응답을 새 DTO로 변환
        var blobResponse = new BlobResponse
        {
            IsSuccess = !result.Error,
            Message = result.Message,
            FileUrl = null
        };
        
        return Ok(blobResponse);
    }
    #endregion

    #region 게시글 테스트
    [HttpGet("post/all")]
    [Authorize(Roles = "NormalUser")]
    [ProducesDefaultResponseType(typeof(IEnumerable<PostResponse>))]
    public async Task<IActionResult> GetAllPost()
    {
        var posts = await _service.PostService.GetAllPost(trackChanges: false);
        
        return Ok(posts);
    }
    #endregion

    #region 좋아요 테스트
    [HttpGet("like/all")]
    [Authorize(Roles = "NormalUser")]
    [ProducesDefaultResponseType(typeof(IEnumerable<LikeResponse>))]
    public async Task<IActionResult> GetAllLike()
    {
        var likes = await _service.LikeService.GetAllLike(trackChanges: false);
        
        // 기존 DTO를 새 DTO로 변환
        var likeResponses = likes.Select(l => new LikeResponse
        {
            LikeId = l.LikeId,
            PostId = l.PostId,
            UserId = l.UserId,
            CreatedDate = l.CreatedDate
        }).ToList();
        
        return Ok(likeResponses);
    }

    [HttpGet("like/{postId:int}", Name = "GetLikeToPostId")]
    [ProducesDefaultResponseType(typeof(IEnumerable<LikeResponse>))]
    public async Task<IActionResult> GetLikeToPostId(int postId)
    {
        var likes = await _service.LikeService.GetLikeToPostId(postId, trackChanges: false);
        
        // 기존 DTO를 새 DTO로 변환
        var likeResponses = likes.Select(l => new LikeResponse
        {
            LikeId = l.LikeId,
            PostId = l.PostId,
            UserId = l.UserId,
            CreatedDate = l.CreatedDate
        }).ToList();

        return Ok(likeResponses);
    }

    [HttpGet("like/num/{postId:int}", Name = "GetLikeNumToPostId")]
    [ProducesDefaultResponseType(typeof(int))]
    public async Task<IActionResult> GetLikeNumToPostId(int postId)
    {
        int likeNum = await _service.LikeService.GetLikeCountToPostId(postId, trackChanges: false);

        return Ok(likeNum);
    }
    #endregion

    #region 댓글 테스트

    #endregion

    #region 프로필 테스트
    [HttpGet("profile/all")]
    [ProducesDefaultResponseType(typeof(IEnumerable<ProfileResponse>))]
    public async Task<IActionResult> GetAllProfile()
    {
        var profiles = await _service.ProfileService.GetAllProfile(trackChanges: false);
        
        return Ok(profiles);
    }
    #endregion

    #region Firebase 테스트
    [HttpPost("firebase/send-message-to-user")]
    public async Task<IActionResult> SendMessageToUser(string userFcmToken, string title, string body)
    {
        var result = await _firebaseAdminSDKService.SendMessageToUser(userFcmToken, title, body);

        return Ok(result);
    }

    [HttpPost("firebase/send-message-to-many-user")]
    public async Task<IActionResult> SendMessageToManyUser(List<string> userFcmTokens, string title, string body)
    {
        var result = await _firebaseAdminSDKService.SendMessageToManyUser(userFcmTokens, title, body);

        return Ok(result);
    }

    [HttpPost("firebase/send-message-to-me")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> SendMessageToMe(string title, string body)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _service.UserService.GetUserById(userId, false);

        var result = await _firebaseAdminSDKService.SendMessageToUser(user.FcmToken, title, body);

        return Ok(result);
    }
    #endregion
}
