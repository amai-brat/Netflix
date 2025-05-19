import 'package:netflix/domain/models/content/ratings.dart';

class ApiRatingsDto {
  final double kinopoiskRating;
  final double imdbRating;
  final double localRating;

  const ApiRatingsDto({
    required this.kinopoiskRating,
    required this.imdbRating,
    required this.localRating,
  });

  ApiRatingsDto.fromMap(Map<String, dynamic> map)
    : kinopoiskRating = (map['kinopoiskRating'] as num).toDouble(),
      imdbRating = (map['imdbRating'] as num).toDouble(),
      localRating = (map['localRating'] as num).toDouble();

  Ratings toRatings() => Ratings(
    kinopoiskRating: kinopoiskRating,
    imdbRating: imdbRating,
    localRating: localRating,
  );
}
