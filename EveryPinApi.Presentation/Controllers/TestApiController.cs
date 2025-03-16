using Azure.Core;
using Entites.Models;
using ExternalLibraryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DataTransferObject;
using Shared.DataTransferObject.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
    [HttpPost("auth/regist")]
    //[ServiceFilter(typeof(ValidationFilterAttribute))]        
    public async Task<IActionResult> RegisterUser([FromBody] RegistUserDto registUserDto)
    {
        var result = await _service.AuthenticationService.RegisterUser(registUserDto);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        var userAccountInfo = await _service.UserService.GetUserByEmail(registUserDto.Email, false);

        var profile = new Entites.Models.Profile()
        {
            UserId = userAccountInfo.Id,
            ProfileName = registUserDto.UserName,
            SelfIntroduction = null,
            PhotoUrl = null,
            ProfileTag = registUserDto.UserName,
            User = userAccountInfo,
            CreatedDate = DateTime.Now
        };

        var createdProfile = await _service.ProfileService.CreateProfile(profile);

        if (createdProfile != null)
        {
            return StatusCode(201);
        }
        else
        {
            return BadRequest("createdProfile가 null입니다.");
        }
    }

    [HttpPost("auth/login")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> Authenticate([FromBody] UserAutenticationDto user)
    {
        if (!await _service.AuthenticationService.ValidateUser(user.Email))
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
        SingleSignOnUserInfo userInfo = await _service.SingleSignOnService.GetUserInfo(platformCode, kakaoAccessToken);
        if (userInfo == null) throw new UnauthorizedAccessException("SSO 인증 실패");

        // 사용자 검증 및 회원가입
        bool isUserExist = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        if (!isUserExist)
        {
            string fcmToken = ""; // 웹 테스트 용도라서 빈문자열로
            var user = await _service.UserService.RegistNewUser(userInfo, fcmToken);
        }

        var tokenDto = await _service.AuthenticationService.CreateTokenWithUpdateFcmToken("", populateExp: true);
        return Ok(tokenDto);
    }

    [HttpGet("auth/google-web-login")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> GoogleWebLogin(string code)
    {
        string platformCode = "google";
        GoogleTokenDto googleAccessToken = await _service.SingleSignOnService.GetGoogleAccessToken(code);

        // 액세스 토큰을 이용하여 플랫폼에서 유저 정보 받아오기
        SingleSignOnUserInfo userInfo = await _service.SingleSignOnService.GetUserInfo(platformCode, googleAccessToken.accessToken);
        if (userInfo == null) throw new UnauthorizedAccessException("SSO 인증 실패");

        // 사용자 검증 및 회원가입
        bool isUserExist = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        if (!isUserExist)
        {
            string fcmToken = ""; // 웹 테스트 용도라서 빈문자열로
            var user = await _service.UserService.RegistNewUser(userInfo, fcmToken);
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
        return Ok(result);
    }

    [HttpPost("blob/upload-blob")]
    public async Task<IActionResult> TestUploadToBlobStorage(IFormFile file)
    {
        var result = await _blobHandlingService.UploadAsync(file);

        if (result.Error)
            return StatusCode(415, result);
        else
            return Ok(result);
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
        return Ok(result);
    }
    #endregion

    #region 게시글 테스트
    [HttpGet("post/all")]
    [Authorize(Roles = "NormalUser")]
    [ProducesDefaultResponseType(typeof(IEnumerable<PostDto>))]
    public async Task<IActionResult> GetAllPost()
    {
        var posts = await _service.PostService.GetAllPost(trackChanges: false);
        return Ok(posts);
    }
    #endregion

    #region 좋아요 테스트
    [HttpGet("like/all")]
    [Authorize(Roles = "NormalUser")]
    [ProducesDefaultResponseType(typeof(LikeDto))]
    public async Task<IActionResult> GetAllLike()
    {
        var likes = await _service.LikeService.GetAllLike(trackChanges: false);
        return Ok(likes);
    }

    [HttpGet("like/{postId:int}", Name = "GetLikeToPostId")]
    [ProducesDefaultResponseType(typeof(IEnumerable<LikeDto>))]
    public async Task<IActionResult> GetLikeToPostId(int postId)
    {
        var likeNum = await _service.LikeService.GetLikeToPostId(postId, trackChanges: false);

        return Ok(likeNum);
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
    [ProducesDefaultResponseType(typeof(IEnumerable<ProfileDto>))]
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
