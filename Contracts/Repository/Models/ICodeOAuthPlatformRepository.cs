using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repository.Models;

public interface ICodeOAuthPlatformRepository
{
    Task<IEnumerable<CodeOAuthPlatform>> GetAllPlatforms(bool trackChanges);
    Task<CodeOAuthPlatform> GetByIdAsync(int platformCode, bool trackChanges);
}