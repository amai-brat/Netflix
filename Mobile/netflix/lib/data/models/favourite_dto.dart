import 'package:netflix/data/models/content_card_dto.dart';
import 'package:netflix/domain/models/favorite.dart';

class FavoriteDto {
  final int id;
  final int? userScore;
  final DateTime addedDate;
  final ContentCardDto content;

  FavoriteDto({
    required this.id,
    required this.userScore,
    required this.addedDate,
    required this.content
  });

  FavoriteDto.fromMap(Map<String, dynamic> map)
      : id = map['id'],
        userScore = map['score'],
        addedDate = map['addedAt'],
        content = ContentCardDto.fromMap(map['contentBase']);

  Favorite toFavorite() => Favorite(
      id: id,
      userScore: userScore,
      addedDate: addedDate,
      content: content.toContent()
  );
}
