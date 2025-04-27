using Contracts.Repository;
using Entites.Exceptions;
using Entites.Models;
using Microsoft.Extensions.Logging;
using Service.Models;
using Shared.Dtos.Like.Responses;
using Shared.Dtos.Like.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

internal sealed class LikeService : ILikeService
{
    private readonly ILogger<LikeService> _logger;
    private readonly IRepositoryManager _repository;

    public LikeService(ILogger<LikeService> logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IEnumerable<LikeResponse>> GetAllLike(bool trackChanges)
    {
        var likes = await _repository.Like.GetAllLike(trackChanges);
        var likeResponses = new List<LikeResponse>();
        
        foreach (var like in likes)
        {
            likeResponses.Add(new LikeResponse
            {
                LikeId = like.LikeId,
                PostId = like.PostId,
                UserId = like.UserId,
                CreatedDate = like.CreatedDate
            });
        }

        return likeResponses;
    }

    public async Task<IEnumerable<LikeResponse>> GetLikeToPostId(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);

        if (post is null)
            throw new PostNotFoundException(postId);

        var likes = await _repository.Like.GetLikeByPostId(postId, trackChanges);
        var likeResponses = new List<LikeResponse>();
        
        foreach (var like in likes)
        {
            likeResponses.Add(new LikeResponse
            {
                LikeId = like.LikeId,
                PostId = like.PostId,
                UserId = like.UserId,
                CreatedDate = like.CreatedDate
            });
        }

        return likeResponses;
    }

    public async Task<int> GetLikeCountToPostId(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);

        if (post is null)
            throw new PostNotFoundException(postId);

        int likeCount = await _repository.Like.GetLikeCountByPostId(postId, trackChanges);

        return likeCount;
    }

    public async Task<LikeResponse> CreateLike(string userId, CreateLikeRequest like)
    {
        var likeEntity = new Like
        {
            PostId = like.PostId,
            UserId = userId,
            CreatedDate = DateTime.Now
        };

        _repository.Like.CreateLike(likeEntity);
        await _repository.SaveAsync();

        var likeResponse = new LikeResponse
        {
            LikeId = likeEntity.LikeId,
            PostId = likeEntity.PostId,
            UserId = likeEntity.UserId,
            CreatedDate = likeEntity.CreatedDate
        };

        return likeResponse;
    }
}
