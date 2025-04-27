using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Shared.Dtos.Profile.Requests;

public class UpdateProfileRequest
{
    public string? ProfileDisplayId { get; set; }
    public string? Name { get; set; }
    public string? SelfIntroduction { get; set; }
    public IFormFile? PhotoFile { get; set; }

    public UpdateProfileRequest FromProfileUploadInputDto(Shared.DataTransferObject.InputDto.ProfileUploadInputDto dto)
    {
        if (dto == null) return null;

        ProfileDisplayId = dto.ProfileDisplayId;
        Name = dto.Name;
        SelfIntroduction = dto.SelfIntroduction;
        PhotoFile = dto.PhotoFiles;

        return this;
    }
}