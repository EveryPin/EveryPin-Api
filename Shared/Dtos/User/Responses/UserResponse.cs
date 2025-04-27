using System;

namespace Shared.Dtos.User.Responses;

public class UserResponse
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int? PlatformCode { get; set; }
    public DateTime CreatedDate { get; set; }
    public Shared.Dtos.Profile.Responses.ProfileResponse Profile { get; set; }

    // Entity에서 Response DTO로 변환 메서드 (AutoMapper 대체)
    public UserResponse FromEntity(Entites.Models.User entity)
    {
        if (entity == null) return null;

        return new UserResponse
        {
            Id = entity.Id,
            UserName = entity.UserName,
            Email = entity.Email,
            PhoneNumber = entity.PhoneNumber,
            PlatformCode = entity.PlatformCode,
            CreatedDate = entity.CreatedDate,
            Profile = entity.Profile != null 
                ? new Shared.Dtos.Profile.Responses.ProfileResponse().FromEntity(entity.Profile) 
                : null
        };
    }
}