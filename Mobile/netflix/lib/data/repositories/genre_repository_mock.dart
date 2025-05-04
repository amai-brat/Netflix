import 'package:netflix/domain/models/genre.dart';
import 'package:netflix/domain/repositories/genre_repository.dart';

class GenreRepositoryMock extends GenreRepository {

  @override
  Future<List<Genre>> getGenres() async {
    await Future.delayed(Duration(milliseconds: 500));
    return [
      Genre(id: 1, name: 'Боевик'),
      Genre(id: 2, name: 'Драма'),
      Genre(id: 3, name: 'Комедия'),
      Genre(id: 4, name: 'Триллер'),
      Genre(id: 5, name: 'Романтика'),
      Genre(id: 6, name: 'Повседневность'),
      Genre(id: 7, name: 'Биография'),
      Genre(id: 8, name: 'Сказка'),
      Genre(id: 9, name: 'Трагедия'),
      Genre(id: 10, name: 'Фантастика')
    ];
  }
}