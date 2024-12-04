using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Features.Contents.Dtos;
using Application.Repositories;
using AutoMapper;

namespace Application.Features.Contents.Queries.Admin.GetMovieContent;

internal class GetMovieContentQueryHandler(
    IMapper mapper,
    IContentRepository contentRepository) : IQueryHandler<GetMovieContentQuery, MovieContentDto>
{
    public async Task<MovieContentDto> Handle(GetMovieContentQuery request, CancellationToken cancellationToken)
    {
        var movieContent = await contentRepository.GetMovieContentByFilterAsync(c => c.Id == request.MovieId);
        if (movieContent is null)
        {
            throw new ArgumentValidationException("такого контента нет");
        }
        
        var movieContentDto = mapper.Map<MovieContentDto>(movieContent);
        return movieContentDto;
    }
}