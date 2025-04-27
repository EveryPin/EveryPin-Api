using System;

namespace Shared.Dtos.Post.Responses;

public class PostPhotoResponse
{
    public int PostPhotoId { get; set; }
    public int PostId { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreatedDate { get; set; }

    public PostPhotoResponse? FromEntity(Entites.Models.PostPhoto entity)
    {
        if (entity == null) return null;

        PostPhotoId = entity.PostPhotoId;
        PostId = entity.PostId;
        PhotoUrl = entity.PhotoUrl;
        UpdateDate = entity.UpdateDate;
        CreatedDate = entity.CreatedDate;

        return this;
    }
}