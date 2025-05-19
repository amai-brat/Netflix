import 'package:netflix/domain/models/content/genre.dart';

class GenreDto {
  final int id;
  final String name;

  const GenreDto({required this.id, required this.name});

  GenreDto.fromMap(Map<String, dynamic> map)
      : id = map['id'],
        name = map['name'];

  Genre toGenre() => Genre(id: id, name: name);
}