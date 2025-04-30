abstract class SubscriptionsEvent {}

class SubscriptionsPageOpened extends SubscriptionsEvent {}

class PurchasePressed extends SubscriptionsEvent {
  final int subscriptionId;
  final String cardNumber;
  final String cardOwner;
  final String validThru;
  final int cvc;

  PurchasePressed({
    required this.subscriptionId,
    required this.cardNumber,
    required this.cardOwner,
    required this.validThru,
    required this.cvc,
  });
}

class UserSubscriptionsUpdated extends SubscriptionsEvent {}
