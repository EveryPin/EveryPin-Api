using Contracts.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repository;

public interface IRepositoryManager
{
    ICommentRepository Comment { get; }
    ILikeRepository Like { get; }
    IPostPhotoRepository PostPhoto { get; }
    IPostRepository Post { get; }
    IProfileRepository Profile { get; }
    IUserRepository User { get; }
    ICodeOAuthPlatformRepository CodeOAuthPlatform { get; }
    Task SaveAsync();
}
