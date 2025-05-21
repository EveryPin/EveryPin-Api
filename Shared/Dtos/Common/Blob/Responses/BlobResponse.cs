using System;

namespace Shared.Dtos.Common.Blob.Responses;

public class BlobResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
}