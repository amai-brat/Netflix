using System.Runtime.Intrinsics.X86;
using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappers;

public class ContentProfile : Profile
{
    public ContentProfile()
    {
        CreateMap<MovieContentAdminPageDto, MovieContent>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => new ContentType { ContentTypeName = src.ContentType }))
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(genre => new Genre { Name = genre }).ToList()))
            .ForMember(dest => dest.PersonsInContent,
                opt => opt.MapFrom(src =>
                    src.PersonsInContent.Select(pdto =>
                        new PersonInContent
                        {
                            Name = pdto.Name,
                            Profession = new Profession { ProfessionName = pdto.Profession }
                        })))
            .ForMember(dest => dest.AllowedSubscriptions,
                opt => opt.MapFrom(src =>
                    src.AllowedSubscriptions.Select(sdto =>
                        new Subscription
                        {
                            Name = sdto.Name,
                            Description = sdto.Description??""
                        })));

        CreateMap<SerialContentAdminPageDto, SerialContent>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => new ContentType { ContentTypeName = src.ContentType }))
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(genre => new Genre { Name = genre }).ToList()))
            .ForMember(dest => dest.PersonsInContent,
                opt => opt.MapFrom(src =>
                    src.PersonsInContent.Select(pdto =>
                        new PersonInContent
                            { Name = pdto.Name, Profession = new Profession { ProfessionName = pdto.Profession } })))
            .ForMember(dest => dest.AllowedSubscriptions,
                opt => opt.MapFrom(src =>
                    src.AllowedSubscriptions.Select(sdto =>
                        new Subscription
                        {
                            Name = sdto.Name,
                            Description = sdto.Description??""
                        })))
            .ForMember(dest => dest.YearRange,
                opt => opt.MapFrom(src
                    => src.ReleaseYears))
            .ForMember(dest => dest.SeasonInfos,
                opt => opt.MapFrom(src =>
                    src.SeasonInfos.Select(sdto => new SeasonInfo
                    {
                        SeasonNumber = sdto.SeasonNumber,
                        Episodes = new List<Episode>(sdto.Episodes.Select(edto => new Episode
                        {
                            VideoUrl = edto.VideoUrl,
                            EpisodeNumber = edto.EpisodeNumber
                        }))
                    })));
    }
}