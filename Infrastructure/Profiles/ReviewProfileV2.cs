using Application.Features.Reviews.Queries.GetReviews;
using Application.Features.Users.Queries.GetReviews;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class ReviewProfileV2 : Profile
{
    public ReviewProfileV2()
    {
        CreateMap<Review, UserReviewDto>()
            .ForMember(x => x.ContentName,
                x => x.MapFrom(rev => rev.Content.Name));

        CreateMap<Review, ReviewDto>()
            .ForMember(x => x.LikesScore,
                x => x.MapFrom(r => r.RatedByUsers!.Count(ur => ur.IsLiked)));

        CreateMap<User, UserDto>()
            .ForMember(x => x.Avatar,
                x => x.MapFrom(u => u.ProfilePictureUrl))
            .ForMember(x => x.Name,
                x => x.MapFrom(u => u.Nickname));

        CreateMap<Comment, CommentDto>();
    }
}