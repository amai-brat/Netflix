using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewDto>()
            .ForMember(x => x.ContentName,
                x => x.MapFrom(rev => rev.Content.Name));
    }
}