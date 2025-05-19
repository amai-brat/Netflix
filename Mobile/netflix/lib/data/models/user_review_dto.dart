import 'package:netflix/domain/models/user_review.dart';

class UserReviewDto {
  final bool isPositive;
  final int score;
  final String text;
  final String contentName;
  final DateTime writtenAt;

  UserReviewDto({
    required this.isPositive,
    required this.score,
    required this.text,
    required this.contentName,
    required this.writtenAt,
  });

  factory UserReviewDto.fromMap(Map<String, dynamic> map) {
    return UserReviewDto(
      isPositive: map['isPositive'] as bool,
      score: map['score'] as int,
      text: map['text'] as String,
      contentName: map['contentName'] as String,
      writtenAt: DateTime.parse(map['writtenAt']),
    );
  }

  UserReview toUserReview(int id) {
    return UserReview(
      isPositive: isPositive,
      score: score,
      text: text,
      contentName: contentName,
      writtenAt: writtenAt,
    );
  }
}