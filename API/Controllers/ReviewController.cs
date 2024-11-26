using API.Helpers;
using Application.Features.Reviews.Commands.AssignReview;
using Application.Features.Reviews.Commands.LikeReview;
using Application.Features.Reviews.Queries.GetReviews;
using Application.Features.Reviews.Queries.GetReviewsCount;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("reviews")]
    [ApiController]
    public class ReviewController(
        IMediator mediator
        ) : ControllerBase
    {

        [HttpGet("{contentId}")]
        public async Task<IActionResult> GetReviewsByContentId(long contentId, [FromQuery] int offset, [FromQuery] int limit, [FromQuery] string sort)
        {
            var dto = await mediator.Send(new GetReviewsQuery(contentId, sort, offset, limit));
            return Ok(dto.Dtos);
        }

        [HttpGet("count/{contentId:long}")]
        public async Task<IActionResult> GetReviewsCount(long contentId)
        {
            var dto = await mediator.Send(new GetReviewsCountQuery(contentId));
            return Ok(dto.Count);
        }

        [HttpPost("assign")]
        [Authorize]
        public async Task<IActionResult> AssignReviewAsync([FromBody] ReviewAssignDto review)
        {
            var userId = this.GetUserId();
            await mediator.Send(new AssignReviewCommand(review, userId));

            return Ok();
        }

        [Authorize]
        [HttpPost("like/{reviewId:long}")]
        public async Task<IActionResult> LikeReviewAsync(long reviewId)
        {
            var userId = this.GetUserId();
            var result = await mediator.Send(new LikeReviewCommand(reviewId, userId));

            return Ok(result.IsSuccessful);
        }
    }
}
