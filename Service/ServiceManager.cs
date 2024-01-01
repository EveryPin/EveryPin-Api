﻿using AutoMapper;
using Contracts.Repository;
using Service.Contracts;
using Service.Contracts.Models;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<ICommentService> _commentService;
        private readonly Lazy<ILikeService> _likeService;
        private readonly Lazy<IPostPhotoService> _postPhotoService;
        private readonly Lazy<IPostService> _postService;
        private readonly Lazy<IProfileService> _profileService;
        //private readonly Lazy<IUserService> _userService;

        public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _commentService = new Lazy<ICommentService>(() => new CommentService(repositoryManager, mapper));
            _likeService = new Lazy<ILikeService>(() => new LikeService(repositoryManager, mapper));
            _postPhotoService = new Lazy<IPostPhotoService>(() => new PostPhotoService(repositoryManager, mapper));
            _postService = new Lazy<IPostService>(() => new PostService(repositoryManager, mapper));
            _profileService = new Lazy<IProfileService>(() => new ProfileService(repositoryManager, mapper));
            //_userService = new Lazy<IUserService>(() => new UserService(repositoryManager));
        }

        public ICommentService CommentService => _commentService.Value;
        public ILikeService LikeService => _likeService.Value;
        public IPostPhotoService PostPhotoService => _postPhotoService.Value;
        public IPostService PostService => _postService.Value;
        public IProfileService ProfileService => _profileService.Value;
        //public IUserService UserService => _userService.Value;
    }
}
