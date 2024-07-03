using Application.Dto;
using Application.Services.Abstractions;
using Infrastructure.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("admin/usermanagement")]
	public class UserManagementController(
		IReviewService reviewService,
		ICommentService commentService,
		IAuthService authService,
		IUserService userService) : Controller
	{
		[Authorize(Roles = "admin, moderator")]
		[HttpDelete("deleteComment/{commentId:long}")]
		public async Task<IActionResult> DeleteUserComment(long commentId)
		{
			return Ok(await commentService.DeleteCommentByIdAsync(commentId));
		}

		[Authorize(Roles = "admin, moderator")]
		[HttpDelete("deleteReview/{reviewId:long}")]
		public async Task<IActionResult> DeleteUserReview(long reviewId)
		{
			return Ok(await reviewService.DeleteReviewByIdAsync(reviewId));
		}

		[Authorize(Roles = "admin")]
		[HttpPatch("changeRole")]
		public async Task<IActionResult> GiveUserDifferentRole([FromBody] UserRoleDto userRoleDto)
		{
			await authService.ChangeRoleAsync(userRoleDto.UserId, userRoleDto.Role);

			return Ok(await userService.GetPersonalInfoAsync(userRoleDto.UserId));
		}
	}
}
