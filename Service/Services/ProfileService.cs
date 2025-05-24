using Contracts.Repository;
using Entites.Models;
using ExternalLibraryService;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Logging;
using Service.Models;
using Shared.Dtos.Profile.Responses;
using Shared.Dtos.Profile.Requests;
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
    private readonly BlobHandlingService _blobHandlingService;

    public ProfileService(ILogger<ProfileService> logger, IRepositoryManager repository, BlobHandlingService blobHandlingService)
    {
        _logger = logger;
        _repository = repository;
        _blobHandlingService = blobHandlingService;
    }

    public async Task<IEnumerable<ProfileResponse>> GetAllProfile(bool trackChanges)
    {
        var profiles = await _repository.Profile.GetAllProfile(trackChanges);
        var profileResponses = new List<ProfileResponse>();
        
        foreach (var profile in profiles)
        {
            var profileResponse = new ProfileResponse();
            profileResponses.Add(profileResponse.FromEntity(profile));
        }

        return profileResponses;
    }

    public async Task<ProfileResponse> RegistNewProfile(User user)
    {
        var newProfile = new Entites.Models.Profile
        {
            User = user,
            UserId = user.Id,
            ProfileName = user.UserName,
            ProfileDisplayId = user.Email.Split('@')[0],
            CreatedDate = DateTime.Now
        };

        var createdProfile = await CreateProfile(newProfile);
        return createdProfile;
    }

    public async Task<ProfileResponse> CreateProfile(Entites.Models.Profile profile)
    {
        if (profile != null)
        {
            _repository.Profile.CreateProfile(profile);
            await _repository.SaveAsync();
        }

        var profileResponse = new ProfileResponse();
        return profileResponse.FromEntity(profile);
    }

    public async Task<ProfileResponse> GetProfileByUserId(string userId, bool trackChanges)
    {
        var profile = await _repository.Profile.GetProfileByUserId(userId, trackChanges);
        var profileResponse = new ProfileResponse();
        return profileResponse.FromEntity(profile);
    }

    public async Task<ProfileResponse> GetProfileByDisplayId(string profileDisplayId, bool trackChanges)
    {
        var profile = await _repository.Profile.GetProfileByDisplayId(profileDisplayId, trackChanges);
        var profileResponse = new ProfileResponse();
        return profileResponse.FromEntity(profile);
    }

    public async Task<ProfileResponse> UpdateProfile(string userId, UpdateProfileRequest updateProfile, bool trackChanges)
    {
        Entites.Models.Profile? originProfile = await _repository.Profile.GetProfileByUserId(userId, false);

        if (originProfile != null)
        {
            originProfile.ProfileDisplayId = updateProfile.ProfileDisplayId;
            originProfile.ProfileName = updateProfile.Name;
            originProfile.SelfIntroduction = updateProfile.SelfIntroduction;

            // 프로필 수정 포함
            if (updateProfile.IsPhotoUpload)
            {
                if (updateProfile.PhotoFile != null)
                {
                    // 프로필을 바꾸는 경우
                    string fileName = $"Profile_{Guid.NewGuid()}";
                    var uploadResult = await _blobHandlingService.UploadImageByFileNameAsync(fileName, updateProfile.PhotoFile);

                    if (uploadResult.Error)
                    {
                        throw new Exception("Blob upload failed: " + uploadResult.Message);
                    }

                    originProfile.OriginPhotoFileName = updateProfile.PhotoFile.FileName;
                    originProfile.UploadedPhotoFileName = fileName;
                    originProfile.PhotoUrl = uploadResult.Blob.Uri;
                }
                else
                {
                    // 프로필을 내리는 경우
                    originProfile.OriginPhotoFileName = null;
                    originProfile.UploadedPhotoFileName = null;
                    originProfile.PhotoUrl = null;
                }
            }

            _repository.Profile.UpdateProfile(originProfile);
            await _repository.SaveAsync();

            var profileResponse = new ProfileResponse();
            return profileResponse.FromEntity(originProfile)!;
        }
        else
        {
            throw new Exception("유저 프로필이 정상적으로 생성되지 않았습니다.");
        }
    }
}
