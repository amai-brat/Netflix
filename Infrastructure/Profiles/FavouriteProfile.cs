using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class FavouriteProfile : Profile
{
    public FavouriteProfile()
    {
        CreateMap<FavouriteContent, FavouriteDto>()
            .ForMember(x => x.ContentBase, 
                x => x.MapFrom(f => f.Content));

        CreateMap<ContentBase, ContentDto>();
        
        CreateMap<FavouriteContent, Application.Features.Users.Queries.GetFavourites.FavouriteDto>()
            .ForMember(x => x.ContentBase, 
                x => x.MapFrom(f => f.Content));

        CreateMap<ContentBase, Application.Features.Users.Queries.GetFavourites.ContentDto>();
    }
}