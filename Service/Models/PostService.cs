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
    internal sealed class PostService : IPostService
    {
        private readonly ILogger<PostService> _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;

        public PostService(ILogger<PostService> logger, IRepositoryManager repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }

        public IEnumerable<PostDto> GetAllPost(bool trackChanges)
        {
            var posts = _repository.Post.GetAllPost(trackChanges);
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);

            return postsDto;
        }

        public PostDto GetPost(int postId, bool trackChanges)
        {
            var post = _repository.Post.GetPost(postId, trackChanges);

            if (post is null) 
                throw new PostNotFoundException(postId);

            var postDto = _mapper.Map<PostDto>(post);

            return postDto;
        }

        public PostDto CreatePost(CreatePostDto post)
        {
            var postEntity = _mapper.Map<Post>(post);

            _repository.Post.CreatePost(postEntity);
            _repository.Save();

            var postToReturn = _mapper.Map<PostDto>(postEntity);

            return postToReturn;
        }
    }
}
