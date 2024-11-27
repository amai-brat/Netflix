using Application.Identity;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, AppUser>()
            .ForMember(x => x.UserName,
                x => x.MapFrom(u => u.Nickname));

        CreateMap<string, AppRole>()
            .ForMember(x => x.Name,
                x => x.MapFrom(s => s));
    }
}