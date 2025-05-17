import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/content_type_dto.dart';
import 'package:netflix/domain/models/content/content_type.dart';
import 'package:netflix/domain/repositories/content_type_repository.dart';

class ContentTypeRepositoryImpl extends ContentTypeRepository {
  final GraphQLClient _client;

  ContentTypeRepositoryImpl(this._client);

  @override
  Future<List<ContentType>> getContentTypes() async {
    const query = r'''
      query GetContentTypes() {
        contentTypes {
          id
          contentTypeName
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

    final contentTypes = result.data?['contentTypes'] ?? [];
    return (contentTypes as List).map((json) => ContentTypeDto.fromMap(json).toContentType()).toList();
  }
}