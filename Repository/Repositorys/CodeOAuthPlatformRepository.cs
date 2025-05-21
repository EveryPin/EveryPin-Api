using Contracts.Repository.Models;
using Entites.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Models;

public class CodeOAuthPlatformRepository : RepositoryBase<CodeOAuthPlatform>, ICodeOAuthPlatformRepository
{
    public CodeOAuthPlatformRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<CodeOAuthPlatform>> GetAllPlatforms(bool trackChanges) =>
        await FindAll(trackChanges)
        .OrderBy(c => c.PlatformCode)
        .ToListAsync();

    public async Task<CodeOAuthPlatform> GetByIdAsync(int platformCode, bool trackChanges) =>
        await FindByCondition(p => p.PlatformCode == platformCode, trackChanges)
        .SingleOrDefaultAsync();
}