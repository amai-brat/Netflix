using Domain.Abstractions;
using Domain.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.CommentController;

[Route("comment")]
[ApiController]
public class CommentController(
    ICommentService commentService,
    INotificationService notificationService
) : ControllerBase
{
    private readonly ICommentService _commentService = commentService;
    private readonly INotificationService _notificationService = notificationService;

    [HttpGet("notifications")]
    [Authorize]
    public async Task<IActionResult> GetAllUserCommentNotifications()
    {
        var userId = User?.FindFirst("id")?.Value;

        var commentNotifications = await _notificationService.GetAllUserCommentNotificationsAsync(long.Parse(userId));
        
        return Ok(SetCommentNotifications(commentNotifications));
    }
    
    [HttpPost("assign")]
    [Authorize]
    public async Task<IActionResult> AssignCommentAsync([FromQuery] long reviewId, [FromBody] CommentAssignDto text)
    {
        var userId = User?.FindFirst("id")?.Value;
            
        var id = await _commentService.AssignCommentAsync(text.Text, long.Parse(userId), reviewId);
            
        return Ok(id);
    }
    
    [HttpPost("set/readed")]
    [Authorize]
    public async Task<IActionResult> AssignCommentAsync([FromQuery] long notificationId)
    {
        await _notificationService.SetNotificationReadedAsync(notificationId);
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