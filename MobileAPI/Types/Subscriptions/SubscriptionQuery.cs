using DataAccess;
using Domain.Entities;
using HotChocolate.Authorization;
using HotChocolate.Language;
using MobileAPI.Helpers;

namespace MobileAPI.Types.Subscriptions;

[ExtendObjectType(OperationType.Query)]
public class SubscriptionQuery
{
    [Authorize]
    public IQueryable<UserSubscription> UserSubscriptions(
        [Service] AppDbContext dbContext, 
        [Service] IHttpContextAccessor accessor)
    {
        var userId = accessor.HttpContext!.GetUserId();
        return dbContext.UserSubscriptions
            .Where(us => us.UserId == userId);
    } 
}