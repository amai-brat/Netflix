class ApiUserSubscription {
  final int id;
  final int userId;
  final int subscriptionId;
  final String expiresAt;
  final String boughtAt;
  final String? transactionId;
  final String status;

  ApiUserSubscription({
    required this.id,
    required this.userId,
    required this.subscriptionId,
    required this.expiresAt,
    required this.boughtAt,
    required this.transactionId,
    required this.status,
  });

  ApiUserSubscription.fromMap(Map<String, dynamic> map)
      : id = map['id'],
        userId = map['userId'],
        subscriptionId = map['subscriptionId'],
        expiresAt = map['expiresAt'],
        boughtAt = map['boughtAt'],
        transactionId = map['transactionId'],
        status = map['status'];
}