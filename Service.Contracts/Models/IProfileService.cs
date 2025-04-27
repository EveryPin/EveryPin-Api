using Entites.Models;
using Shared.Dtos.Profile.Responses;
using Shared.Dtos.Profile.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.Models;

public interface IProfileService
{
    Task<IEnumerable<ProfileResponse>> GetAllProfile(bool trackChanges);
    Task<ProfileResponse> CreateProfile(Profile profile);
    Task<ProfileResponse> GetProfileByUserId(string userId, bool trackChanges);
    Task<ProfileResponse> GetProfileByDisplayId(string profileDisplayId, bool trackChanges);
    Task<ProfileResponse> UpdateProfile(string userId, UpdateProfileRequest updateProfile, bool trackChanges);
    Task<ProfileResponse> RegistNewProfile(User user);
}
