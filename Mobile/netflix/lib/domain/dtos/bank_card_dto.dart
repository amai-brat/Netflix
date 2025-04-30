class BankCardDto {
  final String cardNumber;
  final String cardOwner;
  final String validThru;
  final int cvc;

  BankCardDto({
    required this.cardNumber,
    required this.cardOwner,
    required this.validThru,
    required this.cvc,
  });
}
