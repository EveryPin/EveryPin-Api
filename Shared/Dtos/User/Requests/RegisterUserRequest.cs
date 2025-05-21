using Entites.Models;
using System;
using System.Collections.Generic;

namespace Shared.Dtos.User.Requests;

public class RegisterUserRequest
{
    public string? Name { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int? PlatformCode { get; set; }
    public string? FcmToken { get; set; }
    public ICollection<string>? Roles { get; set; }
    
    public UserData ToUserData()
    {
        return new UserData
        {
            Id = Guid.NewGuid().ToString(),
            UserName = UserName ?? string.Empty,
            Email = Email ?? string.Empty,
            PhoneNumber = PhoneNumber,
            PlatformCode = PlatformCode ?? 0,
            FcmToken = FcmToken,
            CreatedDate = DateTime.Now
        };
    }
}

public class UserData
{
    public string Id { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public int PlatformCode { get; set; }
    public string? FcmToken { get; set; }
    public DateTime CreatedDate { get; set; }
}