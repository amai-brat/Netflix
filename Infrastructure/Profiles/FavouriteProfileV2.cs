using Application.Features.Users.Queries.GetFavourites;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class FavouriteProfileV2 : Profile
{
    public FavouriteProfileV2()
    {
        CreateMap<FavouriteContent, FavouriteDto>()
            .ForMember(x => x.ContentBase, 
                x => x.MapFrom(f => f.Content));

        CreateMap<ContentBase, ContentDto>();
    }
}