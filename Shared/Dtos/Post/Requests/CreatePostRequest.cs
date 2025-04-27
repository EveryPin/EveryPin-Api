using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Shared.Dtos.Post.Requests;

public class CreatePostRequest
{
    public string? PostContent { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public List<IFormFile>? PhotoFiles { get; set; }
}

public class UpdatePostRequest
{
    public string? PostContent { get; set; }
}

public class PostData
{
    public string UserId { get; set; } = string.Empty;
    public string? PostContent { get; set; }
    public float? X { get; set; }
    public float? Y { get; set; }
    public DateTime CreatedDate { get; set; }
}