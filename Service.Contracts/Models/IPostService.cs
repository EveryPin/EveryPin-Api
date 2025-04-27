using Entites.Models;
using Shared.Dtos.Post.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface IPostService
{
    Task<IEnumerable<PostResponse>> GetAllPost(bool trackChanges);
    Task<PostDetailResponse> GetPost(int postId, bool trackChanges);
    Task<PostResponse> CreatePost(Shared.Dtos.Post.Requests.CreatePostRequest post, string userId);
    Task<IEnumerable<PostDetailResponse>> GetSearchPost(double x, double y, double range, bool trackChanges);
}
