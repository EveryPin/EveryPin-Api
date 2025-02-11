using AutoMapper;
using Entites.Models;
using Shared.DataTransferObject;
using Shared.DataTransferObject.OutputDto;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EveryPinApi;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Post, PostDto>();
        CreateMap<CreatePostDto, Post>();
        CreateMap<Post, PostPostPhotoDto>()
        .ForMember(dest => dest.PostPhotos, opt => opt.MapFrom(src => src.PostPhotos));

        CreateMap<PostPhoto, PostPhotoDto>()
            .ForMember(dest => dest.PostPhotoId, opt => opt.MapFrom(src => src.PostPhotoSeq))
            .ForMember(dest => dest.photoUrl, opt => opt.MapFrom(src => src.PhotoUrl));


        CreateMap<Comment, CommentDto>();
        CreateMap<CreateCommentDto, Comment>();
        
        CreateMap<Like, LikeDto>();
        CreateMap<CreateLikeDto, Like>();

        CreateMap<PostPhoto, PostPhotoDto>();
        CreateMap<CreatePostPhotoDto, PostPhoto>();

        //CreateMap<Post, PostDto>()
        //.ForMember(dest => dest.PostPhotos, opt => opt.MapFrom(m => m.PostPhotos))
        //.ForMember(dest => dest.Likes, opt => opt.MapFrom(m => m.Likes))
        //.ForMember(dest => dest.Comments, opt => opt.MapFrom(m => m.Comments));
        
        CreateMap<Entites.Models.Profile, ProfileDto>();
        CreateMap<RegistUserDto, User>();
    }
}
