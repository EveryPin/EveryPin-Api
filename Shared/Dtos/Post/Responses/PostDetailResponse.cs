using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Dtos.Profile.Responses;

namespace Shared.Dtos.Post.Responses;

public class PostDetailResponse
{
    public int PostId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? PostContent { get; set; }
    public double? X { get; set; }
    public double? Y { get; set; }
    public int LikeCount { get; set; }
    public ProfileResponse? Writer { get; set; }
    public ICollection<PostPhotoResponse>? PostPhotos { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public PostDetailResponse FromEntity(
        Entites.Models.Post entity, 
        int likeCount, 
        Entites.Models.Profile? writerProfile)
    {
        if (entity == null) return null;

        PostId = entity.PostId;
        UserId = entity.UserId;
        PostContent = entity.PostContent;
        X = entity.X;
        Y = entity.Y;
        LikeCount = likeCount;
        Writer = writerProfile != null ? new ProfileResponse().FromEntity(writerProfile) : null;
        PostPhotos = entity.PostPhotos?.Count > 0 
            ? entity.PostPhotos.Select(p => new PostPhotoResponse().FromEntity(p)).ToList() 
            : null;
        UpdateDate = entity.UpdateDate;
        CreatedDate = entity.CreatedDate;

        return this;
    }
}