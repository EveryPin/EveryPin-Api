using Entites.Code;
using Entites.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Shared.DataTransferObject;
using Shared.DataTransferObject.Auth;
using Shared.DataTransferObject.InputDto.Auth;
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
    public async Task<IActionResult> Login(LoginInputDto loginInputDto)
    {
        // 액세스 토큰을 이용하여 플랫폼에서 유저 정보 받아오기
        SingleSignOnUserInfo userInfo = await _service.SingleSignOnService.GetUserInfo(loginInputDto.platformCode, loginInputDto.accessToken);
        if (userInfo == null) throw new UnauthorizedAccessException("SSO 인증 실패");

        // 사용자 검증 및 회원가입
        bool isUserExist = await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        if (!isUserExist)
        {
            var user = await _service.UserService.RegistNewUser(userInfo, loginInputDto.fcmToken);
            await _service.AuthenticationService.ValidateUser(userInfo.UserEmail);
        }

        var tokenDto = await _service.AuthenticationService.CreateTokenWithUpdateFcmToken(loginInputDto.fcmToken, populateExp: true);
        return Ok(tokenDto);
    }

    [HttpPost("refresh")]
    [ProducesDefaultResponseType(typeof(TokenDto))]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
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
