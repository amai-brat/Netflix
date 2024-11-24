using Application.Dto;
using Application.Features.CommentNotifications.Commands.SetNotificationReaded;
using Application.Features.CommentNotifications.Queries.GetAllUserCommentNotifications;
using Application.Services.Abstractions;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("comment")]
[ApiController]
public class CommentController(
    ICommentService commentService,
    IMediator mediator
) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;

    [HttpGet("notifications")]
    [Authorize]
    public async Task<IActionResult> GetAllUserCommentNotifications()
    {
        var userId = User.FindFirst("id")?.Value;

        var result = await mediator.Send(new GetAllUserCommentNotificationsQuery(long.Parse(userId!)));
        
        return Ok(SetCommentNotifications(result.Notifications));
    }
    
    [HttpPost("assign")]
    [Authorize]
    public async Task<IActionResult> AssignCommentAsync([FromQuery] long reviewId, [FromBody] CommentAssignDto text)
    {
        var userId = User.FindFirst("id")?.Value;
            
        var id = await _commentService.AssignCommentAsync(text.Text, long.Parse(userId!), reviewId);
            
        return Ok(id);
    }
    
    [HttpPost("set/readed")]
    [Authorize]
    public async Task<IActionResult> AssignCommentAsync([FromQuery] long notificationId)
    {
        await mediator.Send(new SetNotificationReadedCommand(notificationId));
        return Ok();
    }

    private List<CommentNotification> SetCommentNotifications(List<CommentNotification> commentNotifications)
    {
        commentNotifications.ForEach(c =>
        {
            c.Comment.CommentNotification = null!;
            c.Comment.Review.Comments = null!;
            c.Comment.Review.User.Reviews = null!;
            c.Comment.User.Comments = null!;
        });
        return commentNotifications;
    }
}