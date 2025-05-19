import 'package:netflix/domain/models/content/content.dart';

class Favorite {
  final int? userScore;
  final DateTime addedDate;
  final Content content;

  Favorite({
    required this.userScore,
    required this.addedDate,
    required this.content
  });
}
