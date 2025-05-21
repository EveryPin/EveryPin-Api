using System;
using Microsoft.AspNetCore.Http;

namespace Shared.Dtos.Common.Blob.Requests;

public class BlobUploadRequest
{
    public IFormFile File { get; set; }
    public string ContainerName { get; set; }
    public string FolderName { get; set; }
}