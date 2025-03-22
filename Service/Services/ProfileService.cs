using AutoMapper;
using Contracts.Repository;
using Entites.Models;
using Microsoft.Extensions.Logging;
using Service.Models;
using Shared.DataTransferObject;
using Shared.DataTransferObject.InputDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Service.Contracts.Models;

internal sealed class ProfileService : IProfileService
{
    private readonly ILogger<ProfileService> _logger;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public ProfileService(ILogger<ProfileService> logger, IRepositoryManager repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProfileDto>> GetAllProfile(bool trackChanges)
    {
        var profiles = await _repository.Profile.GetAllProfile(trackChanges);
        var profilesDto = _mapper.Map<IEnumerable<ProfileDto>>(profiles);

        return profilesDto;
    }

    public async Task<Entites.Models.Profile> RegistNewProfile(User user)
    {
        var newProfile = new Entites.Models.Profile
        {
            User = user,
            UserId = user.Id,
            ProfileTag = user.UserName,
            ProfileName = user.UserName,
            CreatedDate = DateTime.Now
        };

        return await CreateProfile(newProfile);
    }

    public async Task<Entites.Models.Profile> CreateProfile(Entites.Models.Profile profile)
    {
        if (profile != null)
        {
            _repository.Profile.CreateProfile(profile);
            await _repository.SaveAsync();
        }

        return profile;
    }

    public async Task<Entites.Models.Profile> GetProfileByUserId(string userId, bool trackChanges)
    {
        var profile = await _repository.Profile.GetProfileByUserId(userId, trackChanges);
        //var profileDto = _mapper.Map<ProfileDto>(profile);

        return profile;
    }

    public async Task<Entites.Models.Profile> UpdateProfile(string userId, ProfileInputDto updateProfile)
    {
        Entites.Models.Profile originProfile = await GetProfileByUserId(userId, false);

        if (originProfile != null)
        {
            originProfile.ProfileTag = updateProfile.TagId;
            originProfile.ProfileName = updateProfile.Name;
            originProfile.SelfIntroduction = updateProfile.SelfIntroduction;
            originProfile.PhotoUrl = updateProfile.PhotoUrl;

            _repository.Profile.UpdateProfile(originProfile);
            await _repository.SaveAsync();
        }
    
        return originProfile;
    }
}
