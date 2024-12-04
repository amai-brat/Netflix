using Application.Features.Contents.Dtos;
using Application.Features.Contents.Queries.GetContentTypes;
using Application.Features.Contents.Queries.GetGenres;
using Application.Features.Contents.Queries.GetPromos;
using Application.Features.Contents.Queries.GetSections;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Profiles;

public class ContentProfileV2 : Profile
{
    public ContentProfileV2()
    {
         CreateMap<MovieContentDto, MovieContent>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => new ContentType { ContentTypeName = src.ContentType }))
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(genre => new Genre { Name = genre }).ToList()))
            .ForMember(dest => dest.AgeRatings,
                opt => opt.MapFrom(src => src.AgeRatings != null 
                    ? new AgeRatings { Age = src.AgeRatings.Age, AgeMpaa = src.AgeRatings.AgeMpaa }
                    : null))
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
                            Id = sdto.Id,
                            Name = sdto.Name,
                            Description = sdto.Description ?? "",
                            MaxResolution = sdto.MaxResolution ?? 0
                        })));

        CreateMap<SerialContentDto, SerialContent>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => new ContentType { ContentTypeName = src.ContentType }))
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(genre => new Genre { Name = genre }).ToList()))
            .ForMember(dest => dest.AgeRatings,
                opt => opt.MapFrom(src => src.AgeRating != null 
                    ? new AgeRatings { Age = src.AgeRating.Age, AgeMpaa = src.AgeRating.AgeMpaa }
                    : null))
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
                            Id = sdto.Id,
                            Name = sdto.Name,
                            Description = sdto.Description??"",
                            MaxResolution = sdto.MaxResolution ?? 0
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
        CreateMap<MovieContent, MovieContentDto>()
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()))
            .ForMember(dest => dest.PersonsInContent,
                opt => opt.MapFrom(src => src.PersonsInContent.Select(p => new PersonInContentDto
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
                opt => opt.MapFrom(src => src.AllowedSubscriptions.Select(s => new SubscriptionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    MaxResolution = s.MaxResolution
                }).ToList()));

        CreateMap<SerialContent, SerialContentDto>()
            .ForMember(dest => dest.ContentType,
                opt => opt.MapFrom(src => src.ContentType.ContentTypeName))
            .ForMember(dest => dest.Genres,
                opt => opt.MapFrom(src => src.Genres.Select(g => g.Name).ToList()))
            .ForMember(dest => dest.AgeRating,
                opt => opt.MapFrom(src => src.AgeRatings != null
                    ? new AgeRatings { Age = src.AgeRatings.Age, AgeMpaa = src.AgeRatings.AgeMpaa }
                    : null))
            .ForMember(dest => dest.PersonsInContent,
                opt => opt.MapFrom(src => src.PersonsInContent.Select(p => new PersonInContentDto
                {
                    Name = p.Name,
                    Profession = p.Profession.ProfessionName
                }).ToList()))
            .ForMember(dest => dest.AllowedSubscriptions,
                opt => opt.MapFrom(src => src.AllowedSubscriptions.Select(s => new SubscriptionDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    MaxResolution = s.MaxResolution
                }).ToList()))
            .ForMember(dest => dest.ReleaseYears,
                opt => opt.MapFrom(src => src.YearRange))
            .ForMember(dest => dest.SeasonInfos,
                opt => opt.MapFrom(src => src.SeasonInfos.Select(si => new SeasonInfoDto
                {
                    SeasonNumber = si.SeasonNumber,
                    Episodes = si.Episodes.Select(e => new EpisodeDto
                    {
                        VideoUrl = e.VideoUrl,
                        EpisodeNumber = e.EpisodeNumber
                    }).ToList()
                }).ToList()));

        CreateMap<ContentBase, SectionContentDto>();
        CreateMap<ContentBase, PromoDto>()
            .ForMember(dest => dest.PosterUrl,
                opt => opt.MapFrom(src => src.BigPosterUrl)
            );
        CreateMap<ContentType, ContentTypeDto>()
            .ForMember(x => x.ContentType,
                x => x.MapFrom(ct => ct.ContentTypeName));
        CreateMap<Genre, GenreDto>();
    }
}