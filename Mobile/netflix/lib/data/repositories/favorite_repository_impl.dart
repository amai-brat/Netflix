import 'dart:convert';
import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/favourite_dto.dart';
import 'package:netflix/domain/models/favorite.dart';
import 'package:netflix/domain/models/favorite_filter_params.dart';
import 'package:netflix/domain/repositories/favorite_repository.dart';

class FavoriteRepositoryImpl extends FavoriteRepository {
  final GraphQLClient _client;

  FavoriteRepositoryImpl(this._client);

  @override
  Future<List<Favorite>> getFavorites(FavoriteFilterParams params, int page, int perPage, int removedCount) async {
    const query = r'''
      query GetFavouriteContents($filter: FavouriteFilterInput!, $first: Int, $after: String) {
        favouriteContents (filter: $filter, first: $first, after: $after) {
          nodes {
            score
            addedAt
            contentBase {
              id
              name
              posterUrl
            }
          }
        }
      }
    ''';

    final options = QueryOptions(
      document: gql(query),
      variables: {
        'filter': _createFavouriteFilterArgument(params),
        'first': perPage,
        'after': base64Encode(utf8.encode((page*perPage - 1 - removedCount).toString())),
      },
      fetchPolicy: FetchPolicy.networkOnly
    );

    final result = await _client.query(options);

    if (result.hasException) {
      final error = result.exception!.linkException ?? result.exception!.graphqlErrors.map((e) => e.message).join('\n');
      throw Exception(error);
    }

    final favourites = result.data?['favouriteContents']['nodes'] ?? [];
    return (favourites as List).map((json) => FavoriteDto.fromMap(json).toFavorite()).toList();
  }

  @override
  Future<void> removeFromFavourites(int contentId) async {
    const mutation = r'''
    mutation RemoveFromFavourite($input: RemoveFromFavouriteInput!) {
      removeFromFavourite(input: $input) {
        boolean
      }
    }
    ''';
    final options = MutationOptions(
      document: gql(mutation),
      variables: {
        'input': {'contentId': contentId},
      },
    );
    final result = await _client.mutate(options);

    if (result.hasException) {
      final error = result.exception!.linkException ?? result.exception!.graphqlErrors.map((e) => e.message).join('\n');
      throw Exception(error);
    }
  }

  _createFavouriteFilterArgument(FavoriteFilterParams params) {
    return {
      'name' : params.searchQuery.isEmpty ? null : params.searchQuery,
      'sortBy' : params.sortBy?.stringValue
    };
  }
}