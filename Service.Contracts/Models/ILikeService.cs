using Shared.Dtos.Like.Responses;
using Shared.Dtos.Like.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface ILikeService
{
    Task<IEnumerable<LikeResponse>> GetAllLike(bool trackChanges);
    Task<IEnumerable<LikeResponse>> GetLikeToPostId(int postId, bool trackChanges);
    Task<int> GetLikeCountToPostId(int postId, bool trackChanges);
    Task<LikeResponse> CreateLike(string userId, CreateLikeRequest like);
}
