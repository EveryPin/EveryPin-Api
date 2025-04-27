using Shared.Dtos.Post.Responses;
using Shared.Dtos.Post.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface IPostPhotoService
{
    Task<IEnumerable<PostPhotoResponse>> GetAllPostPhoto(bool trackChanges);
    Task<IEnumerable<PostPhotoResponse>> GetPostPhotoToPostId(int postId, bool trackChanges);
    Task<PostPhotoResponse> CreatePostPhoto(PostPhotoResponse postphoto);
}
