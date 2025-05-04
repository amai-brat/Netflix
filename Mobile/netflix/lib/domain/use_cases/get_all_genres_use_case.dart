import 'package:netflix/domain/models/genre.dart';
import 'package:netflix/domain/repositories/genre_repository.dart';
import 'package:netflix/utils/result.dart';

class GetAllGenresUseCase {
  final GenreRepository _genreRepository;

  GetAllGenresUseCase({required GenreRepository genreRepository})
      : _genreRepository = genreRepository;

  Future<Result<List<Genre>>> execute() async {
    try {
      return Result.ok(await _genreRepository.getGenres());
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}