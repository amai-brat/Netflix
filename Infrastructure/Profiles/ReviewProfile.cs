using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, UserReviewDto>()
            .ForMember(x => x.ContentName,
                x => x.MapFrom(rev => rev.Content.Name));

        CreateMap<Review, Application.Features.Users.Queries.GetReviews.UserReviewDto>()
            .ForMember(x => x.ContentName,
                x => x.MapFrom(rev => rev.Content.Name));

        
        CreateMap<Review, ReviewDto>()
            .ForMember(x => x.LikesScore,
                x => x.MapFrom(r => r.RatedByUsers!.Count(ur => ur.IsLiked)));
    
        CreateMap<Review, Application.Features.Reviews.Queries.GetReviews.ReviewDto>()
            .ForMember(x => x.LikesScore,
                x => x.MapFrom(r => r.RatedByUsers!.Count(ur => ur.IsLiked)));
        
        CreateMap<User, UserDto>()
            .ForMember(x => x.Avatar,
                x => x.MapFrom(u => u.ProfilePictureUrl))
            .ForMember(x => x.Name,
                x => x.MapFrom(u => u.Nickname));

        CreateMap<User, Application.Features.Reviews.Queries.GetReviews.UserDto>()
            .ForMember(x => x.Avatar,
                x => x.MapFrom(u => u.ProfilePictureUrl))
            .ForMember(x => x.Name,
                x => x.MapFrom(u => u.Nickname));
        
        CreateMap<Comment, CommentDto>();
        
        CreateMap<Comment, Application.Features.Reviews.Queries.GetReviews.CommentDto>();
    }
}