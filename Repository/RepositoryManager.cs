using Contracts.Repository;
using Contracts.Repository.Models;
using Entites.Models;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly RepositoryContext _repositoryContext;
    private readonly Lazy<ICommentRepository> _commentRepository;
    private readonly Lazy<ILikeRepository> _likeRepository;
    private readonly Lazy<IPostPhotoRepository> _postPhotoRepository;
    private readonly Lazy<IPostRepository> _postRepository;
    private readonly Lazy<IProfileRepository> _profileRepository;
    private readonly Lazy<IUserRepository> _userRepository;
    private readonly Lazy<ICodeOAuthPlatformRepository> _codeOAuthPlatformRepository;

    public RepositoryManager(RepositoryContext repositoryContext)
    {
        _repositoryContext = repositoryContext;
        _commentRepository = new Lazy<ICommentRepository>(() => new CommentRepository(repositoryContext));
        _likeRepository = new Lazy<ILikeRepository>(() => new LikeRepository(repositoryContext));
        _postPhotoRepository = new Lazy<IPostPhotoRepository>(() => new PostPhotoRepository(repositoryContext));
        _postRepository = new Lazy<IPostRepository>(() => new PostRepository(repositoryContext));
        _profileRepository = new Lazy<IProfileRepository>(() => new ProfileRepository(repositoryContext));
        _userRepository = new Lazy<IUserRepository>(() => new UserRepository(repositoryContext));
        _codeOAuthPlatformRepository = new Lazy<ICodeOAuthPlatformRepository>(() => new CodeOAuthPlatformRepository(repositoryContext));
    }

    public ICommentRepository Comment => _commentRepository.Value;
    public ILikeRepository Like => _likeRepository.Value;
    public IPostPhotoRepository PostPhoto => _postPhotoRepository.Value;
    public IPostRepository Post => _postRepository.Value;
    public IProfileRepository Profile => _profileRepository.Value;
    public IUserRepository User => _userRepository.Value;
    public ICodeOAuthPlatformRepository CodeOAuthPlatform => _codeOAuthPlatformRepository.Value;

    public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
}
