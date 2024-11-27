using Application.Features.Auth.Commands.ChangeRole;
using Application.Features.Comments.Commands.DeleteComment;
using Application.Features.Reviews.Commands.DeleteReview;
using Application.Features.Users.Queries.GetPersonalInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("admin/usermanagement")]
	public class UserManagementController(
		IMediator mediator) : Controller
	{
		[Authorize(Roles = "admin, moderator")]
		[HttpDelete("deleteComment/{commentId:long}")]
		public async Task<IActionResult> DeleteUserComment(long commentId)
		{
			return Ok(await mediator.Send(new DeleteCommentCommand(commentId)));
		}

		[Authorize(Roles = "admin, moderator")]
		[HttpDelete("deleteReview/{reviewId:long}")]
		public async Task<IActionResult> DeleteUserReview(long reviewId)
		{
			return Ok(await mediator.Send(new DeleteReviewCommand(reviewId)));
		}

		[Authorize(Roles = "admin")]
		[HttpPatch("changeRole")]
		public async Task<IActionResult> GiveUserDifferentRole([FromBody] UserRoleDto userRoleDto)
		{
			await mediator.Send(new ChangeRoleCommand(userRoleDto));
			var infoDto = await mediator.Send(new GetPersonalInfoQuery(userRoleDto.UserId));
			return Ok(infoDto);
		}
	}
}
