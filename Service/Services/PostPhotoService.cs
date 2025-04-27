using Contracts.Repository;
using Entites.Exceptions;
using Entites.Models;
using Microsoft.Extensions.Logging;
using Shared.Dtos.Post.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

internal sealed class PostPhotoService : IPostPhotoService
{
    private readonly ILogger<PostPhotoService> _logger;
    private readonly IRepositoryManager _repository;

    public PostPhotoService(ILogger<PostPhotoService> logger, IRepositoryManager repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<IEnumerable<PostPhotoResponse>> GetAllPostPhoto(bool trackChanges)
    {
        var postPhotos = await _repository.PostPhoto.GetAllPostPhoto(trackChanges);
        var postPhotoResponses = new List<PostPhotoResponse>();
        
        foreach (var photo in postPhotos)
        {
            postPhotoResponses.Add(new PostPhotoResponse
            {
                PostPhotoId = photo.PostPhotoId,
                PostId = photo.PostId,
                PhotoUrl = photo.PhotoUrl,
                UpdateDate = photo.UpdateDate,
                CreatedDate = photo.CreatedDate
            });
        }

        return postPhotoResponses;
    }

    public async Task<IEnumerable<PostPhotoResponse>> GetPostPhotoToPostId(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);

        if (post is null)
            throw new PostNotFoundException(postId);

        var postPhotos = await _repository.PostPhoto.GetPostPhotoByPostId(postId, trackChanges);
        var postPhotoResponses = new List<PostPhotoResponse>();
        
        foreach (var photo in postPhotos)
        {
            postPhotoResponses.Add(new PostPhotoResponse
            {
                PostPhotoId = photo.PostPhotoId,
                PostId = photo.PostId,
                PhotoUrl = photo.PhotoUrl,
                UpdateDate = photo.UpdateDate,
                CreatedDate = photo.CreatedDate
            });
        }

        return postPhotoResponses;
    }

    public async Task<PostPhotoResponse> CreatePostPhoto(PostPhotoResponse postphoto)
    {
        var postPhotoEntity = new PostPhoto
        {
            PostId = postphoto.PostId,
            PhotoUrl = postphoto.PhotoUrl,
            CreatedDate = DateTime.Now
        };

        _repository.PostPhoto.CreatePostPhoto(postPhotoEntity);
        await _repository.SaveAsync();

        var postPhotoResponse = new PostPhotoResponse
        {
            PostPhotoId = postPhotoEntity.PostPhotoId,
            PostId = postPhotoEntity.PostId,
            PhotoUrl = postPhotoEntity.PhotoUrl,
            UpdateDate = postPhotoEntity.UpdateDate,
            CreatedDate = postPhotoEntity.CreatedDate
        };

        return postPhotoResponse;
    }
}
