using System;
using Shared.Dtos.User.Responses;
using Shared.Dtos.Profile.Responses;

namespace Shared.Dtos.Follow.Responses;

public class FollowResponse
{
    public int FollowId { get; set; }
    public string FollowerId { get; set; } = string.Empty;
    public string FollowingId { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public ProfileResponse? FollowerProfile { get; set; }
    public ProfileResponse? FollowingProfile { get; set; }

    // Entity에서 변환 메서드
    public FollowResponse FromEntity(
        Entites.Models.Follow entity, 
        Entites.Models.Profile? followerProfile, 
        Entites.Models.Profile? followingProfile)
    {
        if (entity == null) return null;

        FollowId = entity.FollowId;
        FollowerId = entity.FollowerId;
        FollowingId = entity.FollowingId;
        CreatedDate = entity.CreatedDate;
        FollowerProfile = followerProfile != null ? new ProfileResponse().FromEntity(followerProfile) : null;
        FollowingProfile = followingProfile != null ? new ProfileResponse().FromEntity(followingProfile) : null;

        return this;
    }
}