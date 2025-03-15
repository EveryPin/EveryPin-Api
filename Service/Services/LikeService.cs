using AutoMapper;
using Contracts.Repository;
using Entites.Exceptions;
using Entites.Models;
using Microsoft.Extensions.Logging;
using Service.Models;
using Shared.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service.Contracts.Models;

internal sealed class LikeService : ILikeService
{
    private readonly ILogger<LikeService> _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public LikeService(ILogger<LikeService> logger, IRepositoryManager repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<LikeDto>> GetAllLike(bool trackChanges)
    {
        var likes = await _repository.Like.GetAllLike(trackChanges);
        var likesDto = _mapper.Map<IEnumerable<LikeDto>>(likes);

        return likesDto;
    }

    public async Task<IEnumerable<LikeDto>> GetLikeToPostId(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);

        if (post is null)
            throw new PostNotFoundException(postId);

        var likes = await _repository.Like.GetLikeByPostId(postId, trackChanges);
        var likesDto = _mapper.Map<IEnumerable<LikeDto>>(likes);

        return likesDto;
    }

    public async Task<int> GetLikeCountToPostId(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);

        if (post is null)
            throw new PostNotFoundException(postId);

        int likeCount = await _repository.Like.GetLikeCountByPostId(postId, trackChanges);

        return likeCount;
    }

    public async Task<LikeDto> CreateLike(CreateLikeDto like)
    {
        var likeEntity = _mapper.Map<Like>(like);

        _repository.Like.CreateLike(likeEntity);
        await _repository.SaveAsync();

        var likeToReturn = _mapper.Map<LikeDto>(likeEntity);

        return likeToReturn;
    }
}
