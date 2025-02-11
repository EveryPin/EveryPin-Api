using Contracts.Repository.Models;
using Entites.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Repository.Models;

public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
{
    public CommentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Comment>> GetAllComment(bool trackChanges) =>
        await FindAll(trackChanges)
        .OrderBy(comment => comment.UserId)
        .ToListAsync();

    public async Task<IEnumerable<Comment>> GetCommentByPostId(int postId, bool trackChange) =>
        await FindByCondition(comment => comment.PostSeq.Equals(postId), trackChange)
        .OrderBy(comment => comment.CreatedDate)
        .ToListAsync();

    public void CreateComment(Comment comment) =>
        Create(comment);
}
