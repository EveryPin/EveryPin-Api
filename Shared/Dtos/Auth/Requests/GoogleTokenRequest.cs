using System;
using System.Text.Json.Serialization;

namespace Shared.Dtos.Auth.Requests;

public class GoogleTokenRequest
{
    [JsonPropertyName("id_token")]
    public string IdToken { get; set; }
    public string? FcmToken { get; set; }
}