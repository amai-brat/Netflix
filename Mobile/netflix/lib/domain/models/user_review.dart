class UserReview {
  final int id;
  final bool isPositive;
  final String name;
  final int score;
  final String text;
  final String contentName;
  final DateTime writtenAt;

  UserReview({
    required this.id,
    required this.isPositive,
    required this.name,
    required this.score,
    required this.text,
    required this.contentName,
    required this.writtenAt,
  });
}