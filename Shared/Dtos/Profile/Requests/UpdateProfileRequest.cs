using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Shared.Dtos.Profile.Requests;

public class UpdateProfileRequest
{
    public required string ProfileDisplayId { get; set; }
    public required string Name { get; set; }
    public string? SelfIntroduction { get; set; }
    public IFormFile? PhotoFile { get; set; }
    public bool IsPhotoUpload { get; set; }
}