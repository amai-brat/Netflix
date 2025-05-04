import 'package:netflix/domain/models/content/age_ratings.dart';
import 'package:netflix/domain/models/content/budget.dart';
import 'package:netflix/domain/models/content/content_type.dart';
import 'package:netflix/domain/models/content/genre.dart';
import 'package:netflix/domain/models/content/person_in_content.dart';
import 'package:netflix/domain/models/content/ratings.dart';
import 'package:netflix/domain/models/content/trailer_info.dart';

class Content {
  final int id;
  final String title;
  final String posterUrl;
  final double rating;
  final int year;
  final List<Genre> genres;
  final ContentType type;
  final String country;
  final String slogan;
  final AgeRatings ageRatings;
  final Ratings ratings;
  final TrailerInfo trailerInfo;
  final Budget budget;
  final List<PersonInContent> personsInContent;
  final String description;

  const Content({
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
    required this.description
  });
}
