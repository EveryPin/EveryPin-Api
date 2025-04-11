using Entites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repository.Models;

public interface IProfileRepository
{
    Task<IEnumerable<Profile>> GetAllProfile(bool trackChanges);
    void CreateProfile(Profile profile);
    void UpdateProfile(Profile profile);
    Task<Profile> GetProfileByUserId(string userId, bool trackChanges);
    Task<Profile> GetProfileByDisplayId(string profileDisplayId, bool trackChanges);
}
