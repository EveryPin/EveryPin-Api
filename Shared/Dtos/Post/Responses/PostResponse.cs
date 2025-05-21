using System;
using System.Collections.Generic;
using Entites;
using Entites.Models;

namespace Shared.Dtos.Post.Responses;

public class PostResponse
{
    public int PostId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? UserName { get; set; }
    public string? PostContent { get; set; }
    public string? Address { get; set; }
    public double X { get; set; }
    public double Y { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int CommentCount { get; set; }
    public int LikeCount { get; set; }
    public bool IsLiked { get; set; }
    public List<PostPhotoResponse>? Photos { get; set; }
}