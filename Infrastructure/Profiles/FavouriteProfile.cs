using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

[Obsolete("CQRS")]
public class FavouriteProfile : Profile
{
    public FavouriteProfile()
    {
        CreateMap<FavouriteContent, FavouriteDto>()
            .ForMember(x => x.ContentBase, 
                x => x.MapFrom(f => f.Content));

        CreateMap<ContentBase, ContentDto>();
    }
}