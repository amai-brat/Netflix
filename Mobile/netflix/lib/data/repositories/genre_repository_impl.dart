import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/genre_dto.dart';
import 'package:netflix/domain/models/content/genre.dart';
import 'package:netflix/domain/repositories/genre_repository.dart';

class GenreRepositoryImpl extends GenreRepository {
  final GraphQLClient _client;

  GenreRepositoryImpl(this._client);

  @override
  Future<List<Genre>> getGenres() async {
    const query = r'''
      query GetGenres() {
        genres {
          id
          name
        }
      }
    ''';

    final options = QueryOptions(
      document: gql(query),
    );

    final result = await _client.query(options);

    if (result.hasException) {
      final error = result.exception!.linkException ?? result.exception!.graphqlErrors.map((e) => e.message).join('\n');
      throw Exception(error);
    }

    final genres = result.data?['genres'] ?? [];
    return (genres as List).map((json) => GenreDto.fromMap(json).toGenre()).toList();
  }
}