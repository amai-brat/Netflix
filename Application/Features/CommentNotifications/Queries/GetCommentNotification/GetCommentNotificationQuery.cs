using Application.Cqrs.Queries;
using Domain.Entities;

namespace Application.Features.CommentNotifications.Queries.GetCommentNotification;

public record GetCommentNotificationQuery(long CommentId) : IQuery<CommentNotification?>;