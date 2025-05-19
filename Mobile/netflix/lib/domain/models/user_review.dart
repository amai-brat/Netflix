class UserReview {
  final bool isPositive;
  final int score;
  final String text;
  final String contentName;
  final DateTime writtenAt;

  UserReview({
    required this.isPositive,
    required this.score,
    required this.text,
    required this.contentName,
    required this.writtenAt,
  });
}