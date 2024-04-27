using Application.Dto;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

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
            .ForMember(dest => dest.AgeRatings,
                opt => opt.MapFrom(src => src.AgeRatings != null 
                    ? new AgeRatings { Age = src.AgeRatings.Age, AgeMpaa = src.AgeRatings.AgeMpaa }
                    : new AgeRatings()))
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
            .ForMember(dest => dest.AgeRatings,
                opt => opt.MapFrom(src => src.AgeRating != null 
                    ? new AgeRatings { Age = src.AgeRating.Age, AgeMpaa = src.AgeRating.AgeMpaa }
                    : new AgeRatings()))
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
        CreateMap<MovieContent, MovieContentAdminPageDto>()
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()))
            .ForMember(dest => dest.PersonsInContent,
                opt => opt.MapFrom(src => src.PersonsInContent.Select(p => new PersonInContentAdminPageDto
                {
                    Name = p.Name,
                    Profession = p.Profession.ProfessionName
                }).ToList()))
            .ForMember(src => src.ContentType,
                opt => opt.MapFrom(src => src.ContentType.ContentTypeName))
            .ForMember(dest => dest.AgeRatings,
                opt => opt.MapFrom(src => src.AgeRatings != null
                    ? new AgeRatings { Age = src.AgeRatings.Age, AgeMpaa = src.AgeRatings.AgeMpaa }
                    : null))
            .ForMember(dest => dest.AllowedSubscriptions,
                opt => opt.MapFrom(src => src.AllowedSubscriptions.Select(s => new SubscriptionAdminPageDto
                {
                    Name = s.Name,
                    Description = s.Description,
                    MaxResolution = s.MaxResolution
                }).ToList()));

        CreateMap<SerialContent, SerialContentAdminPageDto>()
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => src.ContentType.ContentTypeName))
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()))
            .ForMember(dest => dest.AgeRating,
                opt => opt.MapFrom(src => src.AgeRatings != null
                    ? new AgeRatings { Age = src.AgeRatings.Age, AgeMpaa = src.AgeRatings.AgeMpaa }
                    : null))
            .ForMember(dest => dest.PersonsInContent,
                opt => opt.MapFrom(src => src.PersonsInContent.Select(p => new PersonInContentAdminPageDto
                {
                    Name = p.Name,
                    Profession = p.Profession.ProfessionName
                }).ToList()))
            .ForMember(dest => dest.AllowedSubscriptions,
                opt => opt.MapFrom(src => src.AllowedSubscriptions.Select(s => new SubscriptionAdminPageDto
                {
                    Name = s.Name,
                    Description = s.Description,
                    MaxResolution = s.MaxResolution
                }).ToList()))
            .ForMember(dest => dest.ReleaseYears,
                opt => opt.MapFrom(src => src.YearRange))
            .ForMember(dest => dest.SeasonInfos,
                opt => opt.MapFrom(src => src.SeasonInfos.Select(si => new SeasonInfoAdminPageDto
                {
                    SeasonNumber = si.SeasonNumber,
                    Episodes = si.Episodes.Select(e => new EpisodeAdminPageDto
                    {
                        VideoUrl = e.VideoUrl,
                        EpisodeNumber = e.EpisodeNumber
                    }).ToList()
                }).ToList()));

        CreateMap<ContentBase, SectionContentDto>();
        CreateMap<ContentBase, PromoDto>();
        CreateMap<ContentType, ContentTypeDto>()
            .ForMember(x => x.ContentType,
                x => x.MapFrom(ct => ct.ContentTypeName));
        CreateMap<Genre, GenreDto>();
    }
}