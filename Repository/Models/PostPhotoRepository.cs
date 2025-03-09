using Contracts.Repository.Models;
using Entites.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models;

public class PostPhotoRepository : RepositoryBase<PostPhoto>, IPostPhotoRepository
{
    public PostPhotoRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<PostPhoto>> GetAllPostPhoto(bool trackChanges) =>
        await FindAll(trackChanges)
        .OrderBy(c => c.PostPhotoId)
        .ToListAsync();

    public async Task<IEnumerable<PostPhoto>> GetPostPhotoByPostId(int postId, bool trackChange) =>
        await FindByCondition(postPhoto => postPhoto.PostId.Equals(postId), trackChange)
        .OrderBy(postPhoto => postPhoto.PostId)
        .ToListAsync();

    public void CreatePostPhoto(PostPhoto postphoto) =>
        Create(postphoto);

    public async Task<int> GetLatestPostPhotoId()
    {
        var latestPostPhoto = await FindAll(trackChanges: false)
            .OrderByDescending(c => c.PostPhotoId)
            .FirstOrDefaultAsync();

        return latestPostPhoto?.PostPhotoId ?? 0;
    }
}
