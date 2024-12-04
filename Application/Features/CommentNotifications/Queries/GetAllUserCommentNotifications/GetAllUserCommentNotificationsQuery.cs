using Application.Cqrs.Queries;

namespace Application.Features.CommentNotifications.Queries.GetAllUserCommentNotifications;

public record GetAllUserCommentNotificationsQuery(long UserId) : IQuery<GetAllUserCommentNotificationsDto>;