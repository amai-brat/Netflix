import 'package:netflix/domain/models/content/age_ratings.dart' show AgeRatings;
import 'package:netflix/domain/models/content/budget.dart' show Budget;
import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/models/content/content_type.dart';
import 'package:netflix/domain/models/content/ratings.dart' show Ratings;
import 'package:netflix/domain/models/content/trailer_info.dart' show TrailerInfo;

class ContentCardDto {
  final int id;
  final String title;
  final String posterUrl;

  const ContentCardDto({
    required this.id,
    required this.title,
    required this.posterUrl
  });

  ContentCardDto.fromMap(Map<String, dynamic> map)
      : id = map['id'],
        title = map['name'],
        posterUrl = map['posterUrl'];

  Content toContent() => Content(
    id: id,
    title: title,
    posterUrl: posterUrl,
    rating: 0,
    year: 0,
    genres: const [],
    type: ContentType(id: 0, name: ''),
    country: '',
    slogan: '',
    ageRatings: AgeRatings(age: 0, ageMpaa: ''),
    ratings: Ratings(
      kinopoiskRating: 0,
      imdbRating: 0,
      localRating: 0,
    ),
    trailerInfo: TrailerInfo(url: '', name: ''),
    budget: Budget(budgetValue: 0, budgetCurrencyName: ''),
    personsInContent: const [],
    description: '',
  );
}
