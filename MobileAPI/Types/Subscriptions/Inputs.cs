namespace MobileAPI.Types.Subscriptions;

// {
//   "subscriptionId": 0,
//   "card": {
//     "cardNumber": "string",
//     "cardOwner": "string",
//     "validThru": "string",
//     "cvc": 0
//   }
// }

public record BuySubscriptionInput(int SubscriptionId, CardInput Card);
public record CardInput(string CardNumber, string CardOwner, string ValidThru, int Cvc);

public record CancelSubscriptionInput(int SubscriptionId);