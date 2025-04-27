using System;

namespace Shared.Dtos.Comment.Requests;

public class UpdateCommentRequest
{
    public string? CommentMessage { get; set; }
}