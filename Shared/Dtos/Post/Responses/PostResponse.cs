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

    public PostResponse FromPostDto(Post post)
    {
        if (post == null) return null;

        PostId = post.PostId;
        UserId = post.UserId ?? string.Empty;
        UserName = post.UserName;
        PostContent = post.PostContent;
        Address = post.Address;
        X = post.X;
        Y = post.Y;
        CreatedDate = post.CreatedDate ?? DateTime.Now;
        UpdateDate = post.UpdateDate;
        CommentCount = post.CommentCount;
        LikeCount = post.LikeCount;
        IsLiked = post.IsLiked;
        Photos = post.Photos?.Select(p => new PostPhotoResponse
        {
            PostPhotoId = p.PhotoId,
            PostId = p.PostId,
            PhotoUrl = p.PhotoUrl
        }).ToList();

        return this;
    }
}