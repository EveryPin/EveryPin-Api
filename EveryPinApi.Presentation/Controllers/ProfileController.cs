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

[Route("api/profile")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IServiceManager _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ProfileController(ILogger<ProfileController> logger, IServiceManager service, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("me")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> GetMyProfile()
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var profile = await _service.ProfileService.GetProfileByUserId(userId, trackChanges: false);
        return Ok(profile);
    }

    [HttpGet("user-id/{userId:guid}", Name = "GetProfileByUserId")]
    //[Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> GetProfileByUserId(string userId)
    {
        var profile = await _service.ProfileService.GetProfileByUserId(userId, trackChanges: false);
        return Ok(profile);
    }

    [HttpGet("profile-display-id/{profileDisplayId}")]
    //[Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> GetProfileByDisplayId(string profileDisplayId)
    {
        var profile = await _service.ProfileService.GetProfileByDisplayId(profileDisplayId, trackChanges: false);
        return Ok(profile);
    }

    [HttpPatch("me")]
    [Authorize(Roles = "NormalUser")]
    public async Task<IActionResult> UpdateProfile(ProfileUploadInputDto profileInputDto)
    {
        string userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null)
            return BadRequest("사용자 ID를 찾을 수 없습니다.");
        if (profileInputDto == null)
            return BadRequest("ProfileInputDto가 null 입니다.");

        await _service.ProfileService.UpdateProfile(userId, profileInputDto, trackChanges: true);

        return NoContent(); // 204 No Content, when the update is successful but no data needs to be returned
    }
}
