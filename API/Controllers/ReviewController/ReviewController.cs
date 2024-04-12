using Application.Dto;
using Application.Services.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.ReviewController
{
    [Route("reviews")]
    [ApiController]
    public class ReviewController(
        IReviewService reviewService,
        IContentService contentService
        ) : ControllerBase
    {
        private readonly IContentService _contentService = contentService;

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetReviewsByContentId(long contentId, [FromQuery] int offset, [FromQuery] int limit, [FromQuery] string sort)
        {
            var reviews = await reviewService.GetReviewsByContentIdAsync(contentId, sort, offset, limit);
            reviews.ForEach(SetUpReview);
            return Ok(reviews);
        }

        [HttpPost("assign")]
        [Authorize]
        public async Task<IActionResult> AssignReviewAsync([FromQuery] bool withScore, [FromBody] ReviewAssignDto review)
        {
            if (withScore)
                await reviewService.AssignReviewWithRatingAsync(review, long.Parse(User.FindFirst("Id")!.Value));
            else
                await reviewService.AssignReviewAsync(review, long.Parse(User.FindFirst("Id")!.Value));

            return Ok();
        }

        private void SetUpReview(Review review)
        {
            review.Comments?.ForEach(c =>{
                c.Review = null!;
                c.User.Comments = null!;
                c.ScoredByUsers?.ForEach(s => { s.ScoredComments = null!; });
            });
            review.RatedByUsers?.ForEach(u => u.Review = null!);
            review.User.Reviews = null!;
            review.User.ScoredReviews = null!;
        }
    }
}
