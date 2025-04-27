using System;
using System.Collections.Generic;
using System.Linq;
using Shared.Dtos.Profile.Responses;

namespace Shared.Dtos.Post.Responses;

public class PostDetailResponse
{
    public int PostId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string? PostContent { get; set; }
    public double? X { get; set; }
    public double? Y { get; set; }
    public int LikeCount { get; set; }
    public ProfileResponse? Writer { get; set; }
    public ICollection<PostPhotoResponse>? PostPhotos { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime CreatedDate { get; set; }

    // Entity에서 변환 메서드
    public PostDetailResponse FromEntity(
        Entites.Models.Post entity, 
        int likeCount, 
        Entites.Models.Profile? writerProfile)
    {
        if (entity == null) return null;

        PostId = entity.PostId;
        UserId = entity.UserId;
        PostContent = entity.PostContent;
        X = entity.X;
        Y = entity.Y;
        LikeCount = likeCount;
        Writer = writerProfile != null ? new ProfileResponse().FromEntity(writerProfile) : null;
        PostPhotos = entity.PostPhotos?.Count > 0 
            ? entity.PostPhotos.Select(p => new PostPhotoResponse().FromEntity(p)).ToList() 
            : null;
        UpdateDate = entity.UpdateDate;
        CreatedDate = entity.CreatedDate;

        return this;
    }

    // 기존 DTO에서 변환
    public PostDetailResponse FromPostPostPhotoDto(
        Shared.DataTransferObject.OutputDto.PostPostPhotoDto dto)
    {
        if (dto == null) return null;

        var postPhotos = dto.PostPhotos?.Select(p => 
            new PostPhotoResponse
            {
                PostPhotoId = p.PostPhotoId,
                PostId = dto.PostId, // 사진에는 PostId가 없으므로 포스트 ID를 사용
                PhotoUrl = p.photoUrl, // 소문자 p로 시작하는 속성명 사용
                CreatedDate = dto.CreatedDate ?? DateTime.Now
            }).ToList();

        PostId = dto.PostId;
        UserId = dto.UserId;
        PostContent = dto.PostContent;
        X = dto.x; // 소문자 x 사용
        Y = dto.y; // 소문자 y 사용
        LikeCount = dto.LikeCount;
        Writer = dto.Writer != null ? new ProfileResponse().FromEntity(dto.Writer) : null;
        PostPhotos = postPhotos;
        UpdateDate = dto.UpdateDate;
        CreatedDate = dto.CreatedDate ?? DateTime.Now;

        return this;
    }
}