using AutoMapper;
using Contracts.Repository;
using Entites.Models;
using ExternalLibraryService;
using FirebaseAdmin.Auth;
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
    private readonly BlobHandlingService _blobHandlingService;

    public ProfileService(ILogger<ProfileService> logger, IRepositoryManager repository, IMapper mapper, BlobHandlingService blobHandlingService)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
        _blobHandlingService = blobHandlingService;
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
            ProfileName = user.UserName,
            ProfileDisplayId = user.Email.Split('@')[0],
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

    public async Task<Entites.Models.Profile> GetProfileByDisplayId(string profileDisplayId, bool trackChanges)
    {
        var profile = await _repository.Profile.GetProfileByDisplayId(profileDisplayId, trackChanges);
        //var profileDto = _mapper.Map<ProfileDto>(profile);

        return profile;
    }

    public async Task<Entites.Models.Profile> UpdateProfile(string userId, ProfileUploadInputDto updateProfile, bool trackChanges)
    {
        Entites.Models.Profile originProfile = await GetProfileByUserId(userId, false);

        if (originProfile != null)
        {
            if (updateProfile.PhotoFiles != null)
            {
                var uploadResult = await _blobHandlingService.UploadAsync(updateProfile.PhotoFiles);
                if (uploadResult.Error)
                {
                    throw new Exception("Blob upload failed: " + uploadResult.Message);
                }
                originProfile.PhotoUrl = uploadResult.Blob.Uri;
            }

            originProfile.ProfileName = updateProfile.Name;
            originProfile.SelfIntroduction = updateProfile.SelfIntroduction;

            _repository.Profile.UpdateProfile(originProfile);
            await _repository.SaveAsync();
        }

        return originProfile;
    }
}
