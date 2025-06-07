import 'dart:convert';
import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/api_content_dto.dart';
import 'package:netflix/data/models/api_section_dto.dart';
import 'package:netflix/data/models/content_card_dto.dart';
import 'package:netflix/data/services/metrics_service.dart';
import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/domain/repositories/content_repository.dart';
import 'package:netflix/domain/responses/sections_response.dart';
import 'package:netflix/utils/result.dart';

class ContentRepositoryImpl extends ContentRepository {
  final GraphQLClient _client;
  final MetricsService _metricsService;

  ContentRepositoryImpl(this._client, this._metricsService);

  @override
  Future<List<Content>> getContent(
    ContentFilterParams params,
    int page,
    int perPage,
  ) async {
    const query = r'''
      query GetContents($filter: FilterInput!, $first: Int, $after: String) {
        contents(filter: $filter, first: $first, after: $after) {
          nodes {
            id
            name
            posterUrl
          }
        }
      }
    ''';

    final options = QueryOptions(
      document: gql(query),
      variables: {
        'filter': _createFilterArgument(params),
        'first': perPage,
        'after': base64Encode(utf8.encode((page*perPage - 1).toString())),
      },
    );

    final result = await _client.query(options);

    if (result.hasException) {
      final error = result.exception!.linkException ?? result.exception!.graphqlErrors.map((e) => e.message).join('\n');
      throw Exception(error);
    }

    final contents = result.data?['contents']['nodes'] ?? [];
    return (contents as List).map((json) => ContentCardDto.fromMap(json).toContent()).toList();
  }

  @override
  Future<Result<Content>> getContentById({required int contentId}) async {
    const query = r'''
      query GetContentbyId($id: Long!) {
        contentById(id: $id) {
          id
          name
          description
          slogan
          posterUrl
          country
          ... on MovieContent {
            releaseDate
          }
          contentType {
            id
            contentTypeName
          }
          personsInContent {
            id
            contentId
            name
            profession {
              id
              professionName
            }
          }
          genres {
            id
            name
          }
          trailerInfo {
            url
            name
          }
          budget {
            budgetValue
            budgetCurrencyName
          }
          ratings {
            kinopoiskRating
            imdbRating
            localRating
          }
          ageRatings {
            age
            ageMpaa
          }
        }
      }
    ''';

    final options = QueryOptions(
      document: gql(query),
      variables: {
        'id': contentId
      },
    );

    final result = await _client.query(options);

    if (result.hasException) {
      final error = result.exception!.linkException ?? result.exception!.graphqlErrors.map((e) => e.message).join('\n');
      throw Exception(error);
    }

    final apiContentList = result.data?['contentById'] as List;
    if (apiContentList.isEmpty) {
      return Result.error("Контент не найден");
    }

    return Result.ok(ApiContentDto.fromMap(apiContentList[0]).toContent());
  }

  @override
  Future<Result<SectionsResponse>> getSections() async {
    const query = r'''
      query Sections {
        sections {
          name
          contents {
            id
            name
            posterUrl
          }
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

    final apiSections =  result.data?['sections'] as List;

    return Result.ok(SectionsResponse(data:
      apiSections.map(
              (s) => ApiSectionDto
                  .fromMap(s).toSection())
          .toList()));
  }

  _createFilterArgument(ContentFilterParams params) {
    return {
      'country': params.country,
      'genres': params.selectedGenres.isEmpty ? null : params.selectedGenres.map((g) => g.id).toList(),
      'name': params.searchQuery.isEmpty ? null : params.searchQuery,
      'ratingFrom': params.ratingFrom,
      'ratingTo': params.ratingTo,
      'releaseYearFrom': params.yearFrom,
      'releaseYearTo': params.yearTo,
      'types': params.selectedTypes.isEmpty ? null : params.selectedTypes.map((t) => t.id).toList(),
      'sortBy': params.sortBy?.stringValue
    };
  }

  @override
  Future<({Stream<int> stream, String streamCancellationToken})>
  getContentViewsById({required int contentId}) {
    return _metricsService.getContentViews(contentId);
  }

  @override
  Future<void> sendContentPageOpened({required int contentId}) async {
    await _metricsService.sendContentViewed(contentId: contentId);
  }

  @override
  Future<void> stopContentViewsStream(String streamCancellationToken) async {
    await _metricsService.disconnect(streamCancellationToken: streamCancellationToken);
  }
}
