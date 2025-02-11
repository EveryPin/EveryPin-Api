using Contracts.Repository.Models;
using Entites.Models;
using Microsoft.EntityFrameworkCore;
using Shared.DataTransferObject.InputDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models;

public class PostRepository : RepositoryBase<Post>, IPostRepository
{
    public PostRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Post>> GetAllPost(bool trackChanges) =>
        await FindAll(trackChanges)
        .OrderBy(c => c.PostSeq)
        .ToListAsync();

    public async Task<Post> GetPostById(int postId, bool trackChanges) =>
        await FindByCondition(post => post.PostSeq.Equals(postId), trackChanges)
        .Include(post => post.PostPhotos)
        .SingleOrDefaultAsync();

    public async Task<IEnumerable<Post>> GetSearchPost(double x, double y, double range, bool trackChanges)
    {
        var posts = await FindAll(trackChanges)
            .Where(post => post.X.HasValue &&
                           post.Y.HasValue &&
                           Math.Pow(post.X.Value - x, 2) + Math.Pow(post.Y.Value - y, 2) <= Math.Pow(range, 2))
            .Include(post => post.PostPhotos)
            .OrderBy(c => c.PostSeq)
            .ToListAsync();

        return posts;
    }

    public void CreatePost(Post post) =>
        Create(post);
}
