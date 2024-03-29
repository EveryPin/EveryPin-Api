﻿using AutoMapper;
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

namespace Service.Contracts.Models
{
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

        public IEnumerable<LikeDto> GetAllLike(bool trackChanges)
        {
            var likes = _repository.Like.GetAllLike(trackChanges);
            var likesDto = _mapper.Map<IEnumerable<LikeDto>>(likes);

            return likesDto;
        }

        public IEnumerable<LikeDto> GetLikeToPostId(int postId, bool trackChanges)
        {
            var post = _repository.Post.GetPost(postId, trackChanges);

            if (post is null)
                throw new PostNotFoundException(postId);

            var likes = _repository.Like.GetLikeToPostId(postId, trackChanges);
            var likesDto = _mapper.Map<IEnumerable<LikeDto>>(likes);

            return likesDto;
        }

        public int GetLikeCountToPostId(int postId, bool trackChanges)
        {
            var post = _repository.Post.GetPost(postId, trackChanges);

            if (post is null)
                throw new PostNotFoundException(postId);

            int likeCount = _repository.Like.GetLikeCountToPostId(postId, trackChanges);

            return likeCount;
        }

        public LikeDto CreateLike(CreateLikeDto like)
        {
            var likeEntity = _mapper.Map<Like>(like);

            _repository.Like.CreateLike(likeEntity);
            _repository.Save();

            var likeToReturn = _mapper.Map<LikeDto>(likeEntity);

            return likeToReturn;
        }
    }
}
