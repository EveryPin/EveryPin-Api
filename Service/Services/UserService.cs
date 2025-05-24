using AutoMapper;
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
        try
        {
            // 유저 정보 유효성 검사
            if (userInfo == null)
                throw new ArgumentNullException(nameof(userInfo), "사용자 정보가 null입니다.");

            if (string.IsNullOrWhiteSpace(userInfo.UserNickName) || string.IsNullOrWhiteSpace(userInfo.UserEmail))
                throw new ArgumentException("유효하지 않은 사용자 정보입니다. 닉네임과 이메일은 필수입니다.");

            // 이메일 형식 유효성 검사
            if (!IsValidEmail(userInfo.UserEmail))
                throw new ArgumentException($"올바른 이메일 형식이 아닙니다: {userInfo.UserEmail}");

            // 기존 사용자 확인
            var existingUser = await _repository.User.GetUserByEmail(userInfo.UserEmail, false);
            if (existingUser != null)
            {
                _logger.LogWarning("이미 존재하는 이메일로 회원가입 시도: {Email}", userInfo.UserEmail);
                throw new InvalidOperationException($"이미 가입된 이메일 주소입니다: {userInfo.UserEmail}");
            }

            // 필수 필드가 있는 User 객체 초기화하기 전에 CodeOAuthPlatform 가져오기
            var platformCode = userInfo.PlatformCode;
            var codeOAuthPlatform = await _repository.CodeOAuthPlatform.GetByIdAsync((int)platformCode, false);

            if (codeOAuthPlatform == null)
            {
                _logger.LogError("지원되지 않는 플랫폼 코드: {PlatformCode}", platformCode);
                throw new ArgumentException($"지원되지 않는 플랫폼 코드: {platformCode}");
            }

            // 필수 필드가 있는 User 객체 초기화
            var newUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userInfo.UserNickName,
                Email = userInfo.UserEmail,
                PlatformCode = (int)platformCode,
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
                // 기본 사용자 역할 추가 (프로필 생성 전에 역할 할당)
                var roleResult = await _userManager.AddToRoleAsync(newUser, "NormalUser");
                if (!roleResult.Succeeded)
                {
                    _logger.LogError("사용자 역할 추가 실패: {Errors}", string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    // 사용자는 생성됐지만 역할 추가에 실패했으므로, 생성된 사용자 삭제 시도
                    await _userManager.DeleteAsync(newUser);
                    throw new InvalidOperationException("사용자 역할 할당 실패: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }

                // 역할 할당 성공 후 프로필 생성
                var newProfile = new Entites.Models.Profile()
                {
                    UserId = newUser.Id,
                    ProfileDisplayId = userInfo.UserEmail.Split('@')[0],
                    ProfileName = newUser.UserName,
                    CreatedDate = DateTime.Now,
                    User = newUser
                };

                _repository.Profile.CreateProfile(newProfile);
                await _repository.SaveAsync();

                _logger.LogInformation("새 사용자 등록 성공: {Email}", userInfo.UserEmail);
            }
            else
            {
                _logger.LogError("회원가입 실패: {Errors}", string.Join(", ", userCreateResult.Errors.Select(e => e.Description)));
                throw new InvalidOperationException("회원가입 실패: " + string.Join(", ", userCreateResult.Errors.Select(e => e.Description)));
            }

            // 생성 완료 후 사용자 정보 반환
            var createdUser = await this.GetUserByEmail(userInfo.UserEmail, false);
            if (createdUser == null)
            {
                _logger.LogError("사용자 생성 후 조회 실패: {Email}", userInfo.UserEmail);
                throw new InvalidOperationException("사용자 생성은 되었으나 조회에 실패했습니다.");
            }

            return createdUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "사용자 등록 중 오류 발생: {Message}", ex.Message);
            throw;
        }
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

    // 이메일 형식 검증을 위한 헬퍼 메서드
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
