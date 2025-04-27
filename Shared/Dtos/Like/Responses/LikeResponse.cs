using System;

namespace Shared.Dtos.Like.Responses;

public class LikeResponse
{
    public int LikeId { get; set; }
    public int PostId { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedDate { get; set; }

    public LikeResponse? FromEntity(Entites.Models.Like entity)
    {
        if (entity == null) return null;

        LikeId = entity.LikeId;
        PostId = entity.PostId;
        UserId = entity.UserId;
        CreatedDate = entity.CreatedDate;

        return this;
    }
}