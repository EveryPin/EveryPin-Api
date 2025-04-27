using Entites.Code;
using Entites.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.Dtos.Auth.Requests;
using Shared.Dtos.Auth.Responses;
using Shared.Dtos.Common;
using System.Security.Claims;

namespace EveryPinApi.Presentation.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IServiceManager _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(ILogger<AuthController> logger, IServiceManager service, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("login")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        // 액세스 토큰을 이용하여 플랫폼에서 유저 정보 받아오기
        var userInfo = await _service.SingleSignOnService.GetUserInfo(loginRequest.PlatformCode, loginRequest.AccessToken);
        if (userInfo == null || userInfo.UserEmail == null) throw new UnauthorizedAccessException("SSO 인증 실패");

        // 사용자 검증 및 회원가입
        bool isUserExist = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        if (!isUserExist)
        {
            var user = await _service.UserService.RegistNewUser(userInfo, loginRequest.FcmToken);
            await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        }

        var tokenDto = await _service.AuthenticationService.CreateTokenWithUpdateFcmToken(loginRequest.FcmToken, populateExp: true);

        return Ok(tokenDto);
    }

    [HttpPost("refresh")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest refreshRequest)
    {
        // 필요한 경우 RefreshTokenRequest를 TokenDto로 변환
        var tokenDto = new TokenDto(refreshRequest.AccessToken, refreshRequest.RefreshToken);
        
        var tokenDtoToReturn = await _service.AuthenticationService.RefreshToken(tokenDto);

        return Ok(tokenDtoToReturn);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var result = await _service.AuthenticationService.Logout(userId);

        if (result.Succeeded)
            return Ok();
        else
            return result.Errors.Any(e => e.Code == "InvalidToken") ? Unauthorized() : BadRequest();
    }

    [HttpDelete("user")]
    public async Task<IActionResult> DeleteUser()
    {
        return StatusCode(501);
    }
}
