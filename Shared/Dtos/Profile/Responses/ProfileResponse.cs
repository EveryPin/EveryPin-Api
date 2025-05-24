using System;

namespace Shared.Dtos.Profile.Responses;

public class ProfileResponse
{
    public string? ProfileDisplayId { get; set; } 
    public string? ProfileName { get; set; }
    public string? SelfIntroduction { get; set; }
    public string? OriginPhotoFileName { get; set; }
    public string? UploadedPhotoFileName { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? Email { get; set; }

    public ProfileResponse? FromEntity(Entites.Models.Profile entity)
    {
        if (entity == null) return null;

        ProfileDisplayId = entity.ProfileDisplayId;
        ProfileName = entity.ProfileName;
        SelfIntroduction = entity.SelfIntroduction;
        OriginPhotoFileName = entity.OriginPhotoFileName;
        UploadedPhotoFileName = entity.UploadedPhotoFileName;
        PhotoUrl = entity.PhotoUrl;
        UpdatedDate = entity.UpdatedDate;
        CreatedDate = entity.CreatedDate;
        Email = entity.User.Email;

        return this;
    }
}