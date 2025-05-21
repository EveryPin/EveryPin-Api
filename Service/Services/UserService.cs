using Contracts.Repository;
using Entites.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service.Contracts.Models;
using Shared.Dtos.Common;
using Shared.Dtos.User.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models;

internal sealed class UserService : IUserService
{
    private readonly ILogger _logger;
    private readonly IRepositoryManager _repository;
    private readonly UserManager<User> _userManager;

    public UserService(ILogger<UserService> logger, IRepositoryManager repository, UserManager<User> userManager)
    {
        _logger = logger;
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<User> RegistNewUser(SingleSignOnUserInfo userInfo, string fcmToken)
    {
        if (string.IsNullOrWhiteSpace(userInfo.UserNickName) || string.IsNullOrWhiteSpace(userInfo.UserEmail))
            throw new ArgumentException("유효하지 않은 사용자 정보입니다.");

        // 필수 필드가 있는 User 객체 초기화하기 전에 CodeOAuthPlatform 가져오기
        var platformCode = userInfo.PlatformCode;
        var codeOAuthPlatform = await _repository.CodeOAuthPlatform.GetByIdAsync((int) platformCode, false);
        
        if (codeOAuthPlatform == null)
            throw new Exception($"지원되지 않는 플랫폼 코드: {platformCode}");

        // 필수 필드가 있는 User 객체 초기화
        var newUser = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userInfo.UserNickName,
            Email = userInfo.UserEmail,
            PlatformCode = (int) platformCode,
            CodeOAuthPlatform = codeOAuthPlatform,
            FcmToken = fcmToken,
            CreatedDate = DateTime.UtcNow,
            LastLoginDate = DateTime.UtcNow,
            DeleteCheck = false,
            RefreshTokenExpiryTime = DateTime.UtcNow
        };

        // Identity를 통한 사용자 생성
        var userCreateResult = await _userManager.CreateAsync(newUser, "0");

        if (userCreateResult.Succeeded)
        {
            // 기본 사용자 역할 추가
            await _userManager.AddToRoleAsync(newUser, "NormalUser");
        }
        else
        {
            _logger.LogError("회원가입 실패: {Errors}", string.Join(", ", userCreateResult.Errors.Select(e => e.Description)));
            throw new Exception("회원가입 실패: " + string.Join(", ", userCreateResult.Errors.Select(e => e.Description)));
        }

        return await this.GetUserByEmail(userInfo.UserEmail, false);
    }

    public async Task<User> GetUserByEmail(string email, bool trackChanges)
    {
        var user = await _repository.User.GetUserByEmail(email, trackChanges);

        return user;
    }

    public async Task<User> GetUserById(string userId, bool trackChanges)
    {
        var user = await _repository.User.GetUserById(userId, trackChanges);

        return user;
    }
}
