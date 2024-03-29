using Domain.Abstractions;
using Domain.Dtos;
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
        private readonly IReviewService _reviewService = reviewService;
        private readonly IContentService _contentService = contentService;

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetReviewsByContentId(long contentId, [FromQuery] int offset, [FromQuery] int limit, [FromQuery] string sort)
        {
            if(_contentService.GetContentByIdAsync(contentId) is null) 
                return BadRequest(ErrorMessages.NotFoundContentError(contentId));

            var reviews = await _reviewService.GetReviewsByContentIdAsync(contentId, sort, offset, limit);
            return Ok(reviews);
        }

        [HttpPost("/assign")]
        [Authorize]
        public async Task<IActionResult> AssignReviewAsync([FromQuery] bool withScore, [FromBody] ReviewAssignDto review)
        {
            if(await _contentService.GetContentByIdAsync(review.ContentId) is null)
                return BadRequest(ErrorMessages.NotFoundContentError(review.ContentId));

            if (withScore)
                await _reviewService.AssignReviewWithRatingAsync(review, long.Parse(User.FindFirst("Id")!.Value));
            else
                await _reviewService.AssignReviewAsync(review, long.Parse(User.FindFirst("Id")!.Value));

            return Ok();
        }
    }
}
