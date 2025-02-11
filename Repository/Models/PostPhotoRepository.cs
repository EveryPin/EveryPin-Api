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
        .OrderBy(c => c.PostPhotoSeq)
        .ToListAsync();

    public async Task<IEnumerable<PostPhoto>> GetPostPhotoByPostId(int postId, bool trackChange) =>
        await FindByCondition(postPhoto => postPhoto.PostSeq.Equals(postId), trackChange)
        .OrderBy(postPhoto => postPhoto.PostSeq)
        .ToListAsync();

    public void CreatePostPhoto(PostPhoto postphoto) =>
        Create(postphoto);

    public async Task<int> GetLatestPostPhotoId()
    {
        var latestPostPhoto = await FindAll(trackChanges: false)
            .OrderByDescending(c => c.PostPhotoSeq)
            .FirstOrDefaultAsync();

        return latestPostPhoto?.PostPhotoSeq ?? 0;
    }
}
