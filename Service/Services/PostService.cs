using Contracts.Repository;
using Entites.Exceptions;
using Entites.Models;
using ExternalLibraryService;
using Microsoft.Extensions.Logging;
using Service.Contracts.Models;
using Shared.Dtos.Post.Responses;
using Shared.Dtos.Post.Requests;
using Shared.Dtos.Profile.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

internal sealed class PostService : IPostService
{
    private readonly ILogger<PostService> _logger;
    private readonly IRepositoryManager _repository;
    private readonly BlobHandlingService _blobHandlingService;

    public PostService(ILogger<PostService> logger, IRepositoryManager repository, BlobHandlingService blobHandlingService)
    {
        _logger = logger;
        _repository = repository;
        _blobHandlingService = blobHandlingService;
    }

    public async Task<IEnumerable<PostResponse>> GetAllPost(bool trackChanges)
    {
        var posts = await _repository.Post.GetAllPost(trackChanges);
        var postResponses = new List<PostResponse>();
        
        foreach (var post in posts)
        {
            var likeCount = await _repository.Like.GetLikeCountByPostId(post.PostId, trackChanges);
            var postResponse = new PostResponse
            {
                PostId = post.PostId,
                UserId = post.UserId,
                PostContent = post.PostContent,
                X = post.X ?? 0,
                Y = post.Y ?? 0,
                CreatedDate = post.CreatedDate,
                UpdateDate = post.UpdateDate,
                LikeCount = likeCount,
                Photos = post.PostPhotos?.Select(p => new PostPhotoResponse
                {
                    PostPhotoId = p.PostPhotoId,
                    PostId = p.PostId,
                    PhotoUrl = p.PhotoUrl,
                    UpdateDate = p.UpdateDate,
                    CreatedDate = p.CreatedDate
                }).ToList()
            };
            
            postResponses.Add(postResponse);
        }

        return postResponses;
    }

    public async Task<PostDetailResponse> GetPost(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);
        
        if (post is null) 
            throw new PostNotFoundException(postId);
            
        int likeCount = await _repository.Like.GetLikeCountByPostId(postId, trackChanges);
        var writerProfile = await _repository.Profile.GetProfileByUserId(post.UserId, trackChanges);

        var postDetailResponse = new PostDetailResponse();
        return postDetailResponse.FromEntity(post, likeCount, writerProfile);
    }

    public async Task<IEnumerable<PostDetailResponse>> GetSearchPost(double x, double y, double range, bool trackChanges)
    {
        var posts = await _repository.Post.GetSearchPost(x, y, range, trackChanges);
        var postDetailResponses = new List<PostDetailResponse>();
        
        foreach (var post in posts)
        {
            int likeCount = await _repository.Like.GetLikeCountByPostId(post.PostId, trackChanges);
            var writerProfile = await _repository.Profile.GetProfileByUserId(post.UserId, trackChanges);
            
            var postDetailResponse = new PostDetailResponse();
            postDetailResponses.Add(postDetailResponse.FromEntity(post, likeCount, writerProfile));
        }

        return postDetailResponses;
    }

    public async Task<IEnumerable<PostDetailResponse>> GetPostToProfileDisplayId(string profileDisplayId, bool trackChanges)
    {
        var posts = await _repository.Post.GetPostToProfileDisplayId(profileDisplayId, trackChanges);
        var postDetailResponses = new List<PostDetailResponse>();

        foreach (var post in posts)
        {
            int likeCount = await _repository.Like.GetLikeCountByPostId(post.PostId, trackChanges);
            var writerProfile = await _repository.Profile.GetProfileByUserId(post.UserId, trackChanges);

            var postDetailResponse = new PostDetailResponse();
            postDetailResponses.Add(postDetailResponse.FromEntity(post, likeCount, writerProfile));
        }

        return postDetailResponses;
    }

    public async Task<PostResponse> CreatePost(CreatePostRequest post, string userId)
    {
        // 사용자 정보 조회
        var user = await _repository.User.GetUserById(userId, false);
        if (user == null)
            throw new Exception($"User with id: {userId} does not exist in the database.");

        // Post 엔티티 생성
        var postEntity = new Post
        {
            UserId = userId,
            PostContent = post.PostContent,
            X = (float?)post.X,
            Y = (float?)post.Y,
            CreatedDate = DateTime.Now
        };

        // 업로드된 사진 처리
        var postPhotos = new List<PostPhoto>();
        if (post.PhotoFiles != null && post.PhotoFiles.Count > 0)
        {
            var postPhotoId = await _repository.PostPhoto.GetLatestPostPhotoId();
            foreach (var photo in post.PhotoFiles)
            {
                // Blob 업로드 시도
                var result = await _blobHandlingService.UploadPostPhotoAsync(++postPhotoId, photo);
                if (result.Error)
                {
                    throw new Exception($"Photo upload failed: {result.Message}");
                }
                // PostPhoto 엔티티에 필수 정보 할당
                var postPhoto = new PostPhoto
                {
                    PhotoUrl = result.Blob.Uri,
                    CreatedDate = DateTime.Now
                };
                postPhotos.Add(postPhoto);
            }
        }

        postEntity.PostPhotos = postPhotos;

        _repository.Post.CreatePost(postEntity);
        await _repository.SaveAsync();

        int commentCount = postEntity.Comments?.Count ?? 0;
        int likeCount = postEntity.Likes?.Count ?? 0;

        var postResponse = new PostResponse
        {
            PostId = postEntity.PostId,
            UserId = postEntity.UserId,
            UserName = user.UserName,
            PostContent = postEntity.PostContent,
            X = postEntity.X ?? 0,
            Y = postEntity.Y ?? 0,
            CreatedDate = postEntity.CreatedDate,
            UpdateDate = postEntity.UpdateDate,
            CommentCount = commentCount,
            LikeCount = likeCount,
            Photos = postEntity.PostPhotos?.Select(p => new PostPhotoResponse
            {
                PostPhotoId = p.PostPhotoId,
                PostId = p.PostId,
                PhotoUrl = p.PhotoUrl,
                UpdateDate = p.UpdateDate,
                CreatedDate = p.CreatedDate
            }).ToList()
        };

        return postResponse;
    }
}

