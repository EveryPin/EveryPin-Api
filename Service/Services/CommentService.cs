using Contracts.Repository;
using Service.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entites.Models;
using Microsoft.Extensions.Logging;
using Entites.Exceptions;
using Shared.Dtos.Comment.Responses;
using Shared.Dtos.Comment.Requests;

namespace Service.Models;

internal sealed class CommentService : ICommentService
{
    private readonly ILogger _logger;
    private readonly IRepositoryManager _repository;

    public CommentService(ILogger<CommentService> logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IEnumerable<CommentResponse>> GetAllComment(bool trackChanges)
    {
        var comments = await _repository.Comment.GetAllComment(trackChanges);
        var commentResponses = new List<CommentResponse>();
        
        foreach (var comment in comments)
        {
            commentResponses.Add(new CommentResponse
            {
                CommentId = comment.CommentId,
                PostId = comment.PostId,
                UserId = comment.UserId,
                CommentMessage = comment.CommentMessage,
                UpdateDate = comment.UpdateDate,
                CreatedDate = comment.CreatedDate
            });
        }

        return commentResponses;
    }

    public async Task<IEnumerable<CommentResponse>> GetCommentToPostId(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);

        if (post is null)
            throw new PostNotFoundException(postId);

        var comments = await _repository.Comment.GetCommentByPostId(postId, trackChanges);
        var commentResponses = new List<CommentResponse>();
        
        foreach (var comment in comments)
        {
            commentResponses.Add(new CommentResponse
            {
                CommentId = comment.CommentId,
                PostId = comment.PostId,
                UserId = comment.UserId,
                CommentMessage = comment.CommentMessage,
                UpdateDate = comment.UpdateDate,
                CreatedDate = comment.CreatedDate
            });
        }

        return commentResponses;
    }

    public async Task<CommentResponse> CreateComment(string userId, CreateCommentRequest comment)
    {
        var user = await _repository.User.GetUserById(userId, false);
        var post = await _repository.Post.GetPostById(comment.PostId, false);

        var commentEntity = new Comment
        {
            PostId = comment.PostId,
            Post = post,
            UserId = userId,
            User = user,
            CommentMessage = comment.CommentMessage,
            CreatedDate = DateTime.Now
        };

        _repository.Comment.CreateComment(commentEntity);
        await _repository.SaveAsync();

        var commentResponse = new CommentResponse
        {
            CommentId = commentEntity.CommentId,
            PostId = commentEntity.PostId,
            UserId = commentEntity.UserId,
            CommentMessage = commentEntity.CommentMessage,
            UpdateDate = commentEntity.UpdateDate,
            CreatedDate = commentEntity.CreatedDate
        };

        return commentResponse;
    }
}
