using System;

namespace Shared.Dtos.Auth.Requests;

public class LoginRequest
{
    public required string PlatformCode { get; set; }
    public required string AccessToken { get; set; }
    public required string FcmToken { get; set; }
}