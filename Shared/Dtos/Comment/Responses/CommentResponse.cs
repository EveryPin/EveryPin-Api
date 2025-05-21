using Shared.Dtos.User.Responses;
using System;

namespace Shared.Dtos.Comment.Responses;

public class CommentResponse
{
    public int CommentId { get; set; }
    public int PostId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? CommentMessage { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdateDate { get; set; }
    public UserResponse? User { get; set; }
}