using Contracts.Repository.Models;
using Entites.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models;

public class LikeRepository : RepositoryBase<Like>, ILikeRepository
{
    public LikeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Like>> GetAllLike(bool trackChanges) =>
        await FindAll(trackChanges)
        .OrderBy(c => c.LikeId)
        .ToListAsync();

    public async Task<IEnumerable<Like>> GetLikeByPostId(int postId, bool trackChange) =>
        await FindByCondition(like => like.PostId.Equals(postId), trackChange)
        .ToListAsync();

    public async Task<int> GetLikeCountByPostId(int postId, bool trackChange) =>
        (await FindByCondition(like => like.PostId.Equals(postId), trackChange)
        .ToListAsync()).Count;

    public void CreateLike(Like like) =>
        Create(like);
}
