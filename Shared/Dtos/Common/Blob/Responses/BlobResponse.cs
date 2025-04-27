using System;

namespace Shared.Dtos.Common.Blob.Responses;

public class BlobResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;

    public BlobResponse FromBlobResponseDto(Shared.DataTransferObject.Blob.BlobResponseDto dto)
    {
        if (dto == null) return null;

        IsSuccess = !dto.Error;
        Message = dto.Message ?? string.Empty;
        FileUrl = dto.Blob?.Uri ?? string.Empty;

        return this;
    }
}