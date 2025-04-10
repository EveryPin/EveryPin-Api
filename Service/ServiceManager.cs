using AutoMapper;
using Contracts.Repository;
using Entites.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Service.Contracts;
using Service.Contracts.Models;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Models;
using ExternalLibraryService;

namespace Service;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICommentService> _commentService;
    private readonly Lazy<ILikeService> _likeService;
    private readonly Lazy<IPostPhotoService> _postPhotoService;
    private readonly Lazy<IPostService> _postService;
    private readonly Lazy<IProfileService> _profileService;
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<ISingleSignOnService> _kakaoService;
    private readonly Lazy<IUserService> _userService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, UserManager<User> userManager, IConfiguration configuration, ILoggerFactory loggerFactory, BlobHandlingService blobHandlingService)
    {
        _commentService = new Lazy<ICommentService>(() => new CommentService(loggerFactory.CreateLogger<CommentService>(), repositoryManager, mapper));
        _likeService = new Lazy<ILikeService>(() => new LikeService(loggerFactory.CreateLogger<LikeService>(), repositoryManager, mapper));
        _postPhotoService = new Lazy<IPostPhotoService>(() => new PostPhotoService(loggerFactory.CreateLogger<PostPhotoService>(), repositoryManager, mapper));
        _postService = new Lazy<IPostService>(() => new PostService(loggerFactory.CreateLogger<PostService>(), repositoryManager, mapper, blobHandlingService));
        _profileService = new Lazy<IProfileService>(() => new ProfileService(loggerFactory.CreateLogger<ProfileService>(), repositoryManager, mapper, blobHandlingService));
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(loggerFactory.CreateLogger<AuthenticationService>(), mapper, userManager, configuration));
        _kakaoService = new Lazy<ISingleSignOnService>(() => new SingleSignOnService(loggerFactory.CreateLogger<SingleSignOnService>(), configuration, repositoryManager));
        _userService = new Lazy<IUserService>(() => new UserService(loggerFactory.CreateLogger<UserService>(), repositoryManager, userManager, mapper));
    }

    public ICommentService CommentService => _commentService.Value;
    public ILikeService LikeService => _likeService.Value;
    public IPostPhotoService PostPhotoService => _postPhotoService.Value;
    public IPostService PostService => _postService.Value;
    public IProfileService ProfileService => _profileService.Value;
    public IAuthenticationService AuthenticationService => _authenticationService.Value;
    public ISingleSignOnService SingleSignOnService => _kakaoService.Value;
    public IUserService UserService => _userService.Value;
}
