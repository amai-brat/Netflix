using Application.Dto;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> AssignReviewAsync([FromQuery] bool withScore, [FromBody] ReviewAssignDto review)
        {
            if (withScore)
                await reviewService.AssignReviewWithRatingAsync(review, long.Parse(User.FindFirst("Id")!.Value));
            else
                await reviewService.AssignReviewAsync(review, long.Parse(User.FindFirst("Id")!.Value));

            return Ok();
        }
    }
}
