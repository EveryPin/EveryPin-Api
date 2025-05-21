using System;

namespace Shared.Dtos.Comment.Requests;

public class CreateCommentRequest
{
    public int PostId { get; set; }
    public string? CommentMessage { get; set; }
}