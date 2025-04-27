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
}