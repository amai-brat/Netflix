using Domain.Entities;

namespace MobileAPI.Types.Subscriptions;

public class SubscriptionType : ObjectType<Subscription>
{
    protected override void Configure(IObjectTypeDescriptor<Subscription> descriptor)
    {
        // нужно из-за конфликта: в GraphQL уже есть Subscription
        // см. resolveDataId в graphql_flutter
        descriptor.Name("Subscription_");
        base.Configure(descriptor);
    }
}