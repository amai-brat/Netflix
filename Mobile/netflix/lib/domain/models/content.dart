import 'package:netflix/domain/models/content_type.dart';
import 'package:netflix/domain/models/genre.dart';

class Content {
  final int id;
  final String title;
  final String posterUrl;
  final double rating;
  final int year;
  final List<Genre> genres;
  final ContentType type;
  final String country;

  const Content({
    required this.id,
    required this.title,
    required this.posterUrl,
    required this.rating,
    required this.year,
    required this.genres,
    required this.type,
    required this.country,
  });
}