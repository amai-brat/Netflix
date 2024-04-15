using AutoMapper;
using Domain.Dtos;
using Domain.Entities;

namespace Infrastucture.Profiles;

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