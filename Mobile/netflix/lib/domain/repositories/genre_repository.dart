import 'package:netflix/domain/models/content/genre.dart';

abstract class GenreRepository {
  Future<List<Genre>> getGenres();
}