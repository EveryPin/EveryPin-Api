using Shared.Dtos.Comment.Responses;
using Shared.Dtos.Comment.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface ICommentService
{
    Task<IEnumerable<CommentResponse>> GetAllComment(bool trackChanges);
    Task<IEnumerable<CommentResponse>> GetCommentToPostId(int postId, bool trackChanges);
    Task<CommentResponse> CreateComment(string userId, CreateCommentRequest comment);
}
