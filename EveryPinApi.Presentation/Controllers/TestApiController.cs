using Azure.Core;
using ExternalLibraryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DataTransferObject;
using Shared.DataTransferObject.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EveryPinApi.Presentation.Controllers;

[Route("api/test")]
[ApiController]
public class TestApiController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IServiceManager _service;
    private readonly BlobHandlingService _blobHandlingService;

    public TestApiController(ILogger<TestApiController> logger, IServiceManager service, BlobHandlingService blobHandlingService)
    {
        _logger = logger;
        _service = service;
        _blobHandlingService = blobHandlingService;
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
            Name = null,
            SelfIntroduction = null,
            PhotoUrl = null
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

    [HttpGet("auth/platform-web-login")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> PlatformWebLogin(string code)
    {
        byte platformCode = 2;

        try
        {
            // 액세스 토큰을 이용하여 플랫폼에서 유저 정보 받아오기
            SingleSignOnUserInfo userInfo = null;

            switch (platformCode)
            {
                case 2:
                    string kakaoAccessToken = await _service.SingleSignOnService.GetKakaoAccessToken(code);
                    userInfo = await _service.SingleSignOnService.GetKakaoUserInfo(kakaoAccessToken);
                    break;
                case 3:
                    GoogleTokenDto googleAccessToken = await _service.SingleSignOnService.GetGoogleAccessToken(code);
                    userInfo = await _service.SingleSignOnService.GetGoogleUserInfo(googleAccessToken.accessToken);
                    break;
                default:
                    throw new Exception("유효한 platformCode 값이 아닙니다.");
            }

            // 만일, DB에 해당 email을 가지는 유저가 없으면 회원가입 시키고 유저 식별자와 JWT 반환
            var user = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);

            if (user)
            {
                var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

                return Ok(tokenDto);
            }
            else
            {
                RegistUserDto registerUser = new RegistUserDto()
                {
                    Name = userInfo.UserNickName,
                    UserName = userInfo.UserNickName,
                    Email = userInfo.UserEmail,
                    Password = "0",
                    PlatformCodeId = platformCode,
                    Roles = new List<string>() { "NormalUser" }
                };

                IdentityResult registedUser = await _service.AuthenticationService.RegisterUser(registerUser);

                if (registedUser.Succeeded && await _service.AuthenticationService.ValidateUser(userInfo.UserEmail))
                {
                    var userAccountInfo = await _service.UserService.GetUserByEmail(userInfo.UserEmail, false);

                    var profile = new Entites.Models.Profile()
                    {
                        UserId = userAccountInfo.Id,
                        Name = null,
                        SelfIntroduction = null,
                        PhotoUrl = null
                    };

                    var createdProfile = await _service.ProfileService.CreateProfile(profile);

                    if (createdProfile != null)
                    {
                        var tokenDto = await _service.AuthenticationService.CreateToken(populateExp: true);

                        return Ok(tokenDto);
                    }
                    else
                    {
                        _logger.LogError($"테스트 프로필 생성 실패 - userId: {userAccountInfo.Id}");
                        return BadRequest("테스트 프로필 생성에 실패하였습니다.");
                    }
                }
                else
                {
                    foreach (var error in registedUser.Errors)
                    {
                        _logger.LogError($"테스트 Code: {error.Code}, Description: {error.Description}");
                    }

                    return Unauthorized();
                }
            }

        }
        catch (Exception ex)
        {
            return Unauthorized();
        }
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

}
