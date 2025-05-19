import 'package:netflix/data/models/api_budget_dto.dart';
import 'package:netflix/data/models/api_content_type_dto.dart';
import 'package:netflix/data/models/api_person_in_content_dto.dart';
import 'package:netflix/data/models/api_ratings_dto.dart';
import 'package:netflix/data/models/api_trailer_info.dart';
import 'package:netflix/data/models/genre_dto.dart';
import 'package:netflix/domain/models/content/content.dart';

import 'api_age_ratings_dto.dart';

class ApiContentDto {
  final int id;
  final String title;
  final String posterUrl;
  final double rating;
  final int year;
  final List<GenreDto> genres;
  final ApiContentTypeDto type;
  final String country;
  final String slogan;
  final ApiAgeRatingsDto ageRatings;
  final ApiRatingsDto ratings;
  final ApiTrailerInfo trailerInfo;
  final ApiBudgetDto budget;
  final List<ApiPersonInContentDto> personsInContent;
  final String description;

  const ApiContentDto({
    required this.id,
    required this.title,
    required this.posterUrl,
    required this.rating,
    required this.year,
    required this.genres,
    required this.type,
    required this.country,
    required this.slogan,
    required this.ageRatings,
    required this.ratings,
    required this.trailerInfo,
    required this.budget,
    required this.personsInContent,
    required this.description,
  });

  ApiContentDto.fromMap(Map<String, dynamic> map)
    : id = map['id'],
      title = map['name'],
      posterUrl = map['posterUrl'],
      rating = 0,
      year = DateTime.parse(map['releaseDate']).year,
      genres = (map['genres'] as List).map((g) => GenreDto.fromMap(g)).toList(),
      type = ApiContentTypeDto.fromMap(map['contentType']),
      country = map['country'],
      slogan = map['slogan'],
      ageRatings = ApiAgeRatingsDto.fromMap(map['ageRatings']),
      description = map['description'],
      budget = ApiBudgetDto.fromMap(map['budget']),
      ratings = ApiRatingsDto.fromMap(map['ratings']),
      trailerInfo = ApiTrailerInfo.fromMap(map['trailerInfo']),
      personsInContent =
          (map['personsInContent'] as List)
              .map((p) => ApiPersonInContentDto.fromMap(p))
              .toList();

  Content toContent() => Content(
    id: id,
    title: title,
    posterUrl: posterUrl,
    rating: 0,
    year: year,
    genres: genres.map((g) => g.toGenre()).toList(),
    type: type.toContentType(),
    country: country,
    slogan: slogan,
    ageRatings: ageRatings.toAgeRatings(),
    ratings: ratings.toRatings(),
    trailerInfo: trailerInfo.toTrailerInfo(),
    budget: budget.toBudget(),
    personsInContent:
        personsInContent.map((p) => p.toPersonInContent()).toList(),
    description: description,
  );
}
