import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/user_review_dto.dart';
import 'package:netflix/domain/models/user_review.dart';
import 'package:netflix/utils/result.dart';

class ReviewsService{
  final GraphQLClient _client;
  static const int pageSize = 5;

  ReviewsService(this._client);

  Result<T> _getErrorResult<T>(QueryResult<Object?> queryResult) {
    if (queryResult.exception?.linkException != null) {
      return Result.error(queryResult.exception!.linkException.toString());
    }
    return Result.error(
      queryResult.exception!.graphqlErrors.map((x) => x.message).join('\n'),
    );
  }

  Future<Result<List<UserReview>>> getReviews({
    required int page,
    String? search,
    String? sort,
  }) async {
    const query = r'''
      query GetReviews($skip: Int!, $take: Int!, $where: UserReviewDtoFilterInput, $order: [UserReviewDtoSortInput!]) {
        reviews(skip: $skip, take: $take, where: $where, order: $order) {
          items {
            isPositive
            score
            text
            contentName
            writtenAt
          }
          pageInfo {
            hasNextPage
            hasPreviousPage
          }
        }
      }
    ''';

    final variables = <String, dynamic>{
      'skip': (page - 1) * pageSize,
      'take': pageSize,
    };

    if (search != null && search.isNotEmpty) {
      variables['where'] = {
        'text': {
          'contains': search,
        },
      };
    }

    if (sort == 'date') {
      variables['order'] = [
        {'writtenAt': 'DESC'}
      ];
    } else if (sort == 'score') {
      variables['order'] = [
        {'score': 'DESC'}
      ];
    }

    final options = QueryOptions(
      document: gql(query),
      variables: variables,
      fetchPolicy: FetchPolicy.networkOnly,
    );

    try {
      final result = await _client.query(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      final rawItems = result.data?['reviews']?['items'];
      if (rawItems == null || rawItems is! List) {
        return Result.error('Некорректный ответ от сервера');
      }

      final reviews = List<UserReview>.generate(
        rawItems.length,
            (index) {
          final dto = UserReviewDto.fromMap(rawItems[index]);
          return dto.toUserReview(index + (page - 1) * pageSize);
        },
      );

      return Result.ok(reviews);
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<int>> getTotalPages({String? search}) async {
    const query = r'''
      query ReviewsPagesCount($search: String!) {
        reviewsPagesCount(search: $search)
      }
    ''';

    final options = QueryOptions(
      document: gql(query),
      variables: {
        'search': search ?? '',
      },
      fetchPolicy: FetchPolicy.networkOnly,
    );

    try {
      final result = await _client.query(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      final pages = result.data?['reviewsPagesCount'];
      if (pages is int) {
        return Result.ok(pages);
      } else {
        return Result.error('Некорректный формат данных');
      }
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}