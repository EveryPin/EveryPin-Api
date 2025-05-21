using System;

namespace Shared.Dtos.Like.Requests;

public class CreateLikeRequest
{
    public int PostId { get; set; }
}

public class DeleteLikeRequest
{
    public int PostId { get; set; }
}