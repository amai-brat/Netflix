using Application.Dto;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("reviews")]
    [ApiController]
    public class ReviewController(
        IReviewService reviewService,
        IContentService contentService
        ) : ControllerBase
    {
        // ReSharper disable once UnusedMember.Local
        private readonly IContentService _contentService = contentService;

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetReviewsByContentId(long contentId, [FromQuery] int offset, [FromQuery] int limit, [FromQuery] string sort)
        {
            var reviews = await reviewService.GetReviewsByContentIdAsync(contentId, sort, offset, limit);
            return Ok(reviews);
        }

        [HttpGet("count/{contentId:long}")]
        public async Task<IActionResult> GetReviewsCount(long contentId)
        {
            var count = await reviewService.GetReviewsCountByContentIdAsync(contentId);
            return Ok(count);
        }

        [HttpPost("assign")]
        [Authorize]
        public async Task<IActionResult> AssignReviewAsync([FromBody] ReviewAssignDto review)
        {
            if (review.Score != null)
                await reviewService.AssignReviewWithRatingAsync(review, long.Parse(User.FindFirst("Id")!.Value));
            else
                await reviewService.AssignReviewAsync(review, long.Parse(User.FindFirst("Id")!.Value));

            return Ok();
        }

        [Authorize]
        [HttpPost("like/{reviewId:long}")]
        public async Task<IActionResult> LikeReviewAsync(long reviewId)
        {
            var userId = GetUserId();
            var result = await reviewService.LikeReviewAsync(reviewId, userId);

            return Ok(result);
        }
        
        private long GetUserId()
        {
            return long.Parse(HttpContext.User.FindFirst("id")!.Value);
        }
    }
}
