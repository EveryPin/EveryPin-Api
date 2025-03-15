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

internal sealed class PostPhotoService : IPostPhotoService
{
    private readonly ILogger<PostPhotoService> _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public PostPhotoService(ILogger<PostPhotoService> logger, IRepositoryManager repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PostPhotoDto>> GetAllPostPhoto(bool trackChanges)
    {
        var postPhotos = await _repository.PostPhoto.GetAllPostPhoto(trackChanges);
        var postPhotosDto = _mapper.Map<IEnumerable<PostPhotoDto>>(postPhotos);

        return postPhotosDto;
    }

    public async Task<IEnumerable<PostPhotoDto>> GetPostPhotoToPostId(int postId, bool trackChanges)
    {
        var post = await _repository.Post.GetPostById(postId, trackChanges);

        if (post is null)
            throw new PostNotFoundException(postId);

        var postPhotosFromDb = _repository.PostPhoto.GetPostPhotoByPostId(postId, trackChanges);
        var postPhotosDto = _mapper.Map<IEnumerable<PostPhotoDto>>(postPhotosFromDb);

        return postPhotosDto;
    }

    public async Task<PostPhotoDto> CreatePostPhoto(CreatePostPhotoDto postphoto)
    {
        var postPhotoEntity = _mapper.Map<PostPhoto>(postphoto);

        _repository.PostPhoto.CreatePostPhoto(postPhotoEntity);
        await _repository.SaveAsync();

        var postPhotoToReturn = _mapper.Map<PostPhotoDto>(postPhotoEntity);

        return postPhotoToReturn;
    }
}
