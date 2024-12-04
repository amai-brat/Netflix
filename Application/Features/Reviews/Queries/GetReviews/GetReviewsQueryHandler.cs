using Application.Cache;
using Application.Cqrs.Queries;
using Application.Exceptions.Base;
using Application.Exceptions.ErrorMessages;
using Application.Providers;
using Application.Repositories;
using AutoMapper;
using Domain.Entities;

namespace Application.Features.Reviews.Queries.GetReviews;

internal class GetReviewsQueryHandler(
    IReviewRepository reviewRepository,
    IMinioCache minioCache,
    IMapper mapper,
    IProfilePicturesProvider profilePicturesProvider
    ) : IQueryHandler<GetReviewsQuery, GetReviewsDto>
{
    public async Task<GetReviewsDto> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await GetReviewsByContentIdAndSortAsync(request.ContentId, request.Sort);
        // current user profile pic url are just guid strings. they must be converted into presigned urls
        // but it would be kind of expensive so we'll cache it guid-url to redis for 1 hour

        foreach (var review in reviews)
        {
            if (review.User.ProfilePictureUrl == null)
            {
                continue;
            }

            var picture = await minioCache.GetStringAsync(review.User.ProfilePictureUrl);
            if (picture == null)
            {
                // это случай если картинка из oauth. тогда ее не кешируем и не преобразуем в url
                if (review.User.ProfilePictureUrl.StartsWith("http"))
                {
                    continue;
                }
                review.User.ProfilePictureUrl = await profilePicturesProvider.GetUrlAsync(review.User.ProfilePictureUrl);
                await minioCache.SetStringAsync(review.User.ProfilePictureUrl, review.User.ProfilePictureUrl);
            }

            if (picture != null)
            {
                review.User.ProfilePictureUrl = picture;
            }
        }
			
        var reviewDtos = mapper.Map<List<ReviewDto>>(reviews);
        return new GetReviewsDto
        {
            Dtos = reviewDtos[
                Math.Min(reviews.Count, request.Offset)..Math.Min(reviews.Count, request.Offset + request.Limit)
            ]
        };
    }
    
    private async Task<List<Review>> GetReviewsByContentIdAsync(long contentId) =>
        await reviewRepository.GetReviewsByFilterAsync(r => r.ContentId == contentId);

    private async Task<List<Review>> GetReviewsByContentIdAndSortAsync(long contentId, string sort) => 
        ((sort.ToLower(), await GetReviewsByContentIdAsync(contentId)) switch
        {
            ("score", var reviews) => reviews.OrderBy(r => r.Score),
            ("scoredesc", var reviews) => reviews.OrderByDescending(r => r.Score),
            ("oldest", var reviews) => reviews.OrderBy(r => r.WrittenAt),
            ("newest", var reviews) => reviews.OrderByDescending(r => r.WrittenAt),
            ("positive", var reviews) => reviews.OrderByDescending(r => r.IsPositive),
            ("negative", var reviews) => reviews.OrderBy(r => r.IsPositive),
            ("likes", var reviews) => reviews.OrderBy(r => r.RatedByUsers?.Count(usersReviews => usersReviews.IsLiked) ?? 0),
            ("likesdesc", var reviews) => reviews.OrderByDescending(r => r.RatedByUsers?.Count(usersReviews => usersReviews.IsLiked) ?? 0),
            var (_, _) => throw new ArgumentValidationException(ErrorMessages.IncorrectSortType, sort)
        }).ToList();
}