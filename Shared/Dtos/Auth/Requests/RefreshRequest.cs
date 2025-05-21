using System;

namespace Shared.Dtos.Auth.Requests;

public class RefreshRequest
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}