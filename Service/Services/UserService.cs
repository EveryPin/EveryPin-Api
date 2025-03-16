using AutoMapper;
using Contracts.Repository;
using Entites.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Service.Contracts.Models;
using Shared.DataTransferObject;
using Shared.DataTransferObject.Auth;
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
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public UserService(ILogger<UserService> logger, IRepositoryManager repository, UserManager<User> userManager, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<User> RegistNewUser(SingleSignOnUserInfo userInfo, string fcmToken)
    {
        var newUser = new RegistUserDto
        {
            Name = userInfo.UserNickName,
            UserName = userInfo.UserNickName,
            Email = userInfo.UserEmail,
            Password = "0",
            PlatformCode = (int)userInfo.PlatformCode,
            FcmToken = fcmToken,
            Roles = new List<string> { "NormalUser" },
            CreatedDate = DateTime.Now
        };

        var user = _mapper.Map<User>(newUser);

        var profile = new Entites.Models.Profile()
        {
            UserId = user.Id,
            ProfileTag = user.UserName,
            ProfileName = user.UserName,
            CreatedDate = DateTime.Now,
            User = user
        };
        user.Profile = profile;

        var userCreateresult = await _userManager.CreateAsync(user, newUser.Password ?? "0");

        if (userCreateresult.Succeeded)
            await _userManager.AddToRolesAsync(user, newUser.Roles);
        else
            throw new Exception("회원가입 실패");

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
