import 'package:netflix/domain/models/genre.dart';

abstract class GenreRepository {
  Future<List<Genre>> getGenres();
}