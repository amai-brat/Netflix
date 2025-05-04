import 'package:collection/collection.dart';
import 'package:netflix/domain/models/content.dart';
import 'package:netflix/domain/models/content_type.dart';
import 'package:netflix/domain/models/favorite.dart';
import 'package:netflix/domain/models/favorite_filter_params.dart';
import 'package:netflix/domain/models/genre.dart';
import 'package:netflix/domain/repositories/favorite_repository.dart';

class FavoriteRepositoryMock extends FavoriteRepository {
  
  @override
  Future<List<Favorite>> getFavorites(FavoriteFilterParams params, int page, int perPage) async {
    await Future.delayed(Duration(seconds: 1));
    const allContent = [
      Content(
        id: 1,
        title: 'Aboba',
        posterUrl: 'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
        rating: 7.5,
        year: 2020,
        genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
        type: ContentType(id: 1, name: 'Фильм'),
        country: 'США',
      ),
      Content(
        id: 2,
        title: 'Biba',
        posterUrl: 'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
        rating: 5,
        year: 2024,
        genres: [Genre(id: 2, name: 'Драма'), Genre(id: 3, name: 'Комедия')],
        type: ContentType(id: 3, name: 'Мультфильм'),
        country: 'США',
      ),
      Content(
        id: 3,
        title: 'Boba',
        posterUrl: 'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
        rating: 1,
        year: 2000,
        genres: [Genre(id: 10, name: 'Фантастика'), Genre(id: 2, name: 'Драма')],
        type: ContentType(id: 2, name: 'Сериал'),
        country: 'США',
      ),
      Content(
        id: 4,
        title: 'Tralalelo',
        posterUrl: 'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
        rating: 9,
        year: 2014,
        genres: [Genre(id: 6, name: 'Повседневность'), Genre(id: 4, name: 'Триллер')],
        type: ContentType(id: 1, name: 'Фильм'),
        country: 'США',
      ),
      Content(
        id: 5,
        title: 'Bambini',
        posterUrl: 'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
        rating: 5.2,
        year: 2019,
        genres: [Genre(id: 6, name: 'Повседневность'), Genre(id: 9, name: 'Трагедия')],
        type: ContentType(id: 3, name: 'Мультфильм'),
        country: 'США',
      ),
      Content(
        id: 6,
        title: 'Gussini',
        posterUrl: 'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
        rating: 5.9,
        year: 2014,
        genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
        type: ContentType(id: 2, name: 'Сериал'),
        country: 'США',
      )
    ];
    final favoriteData = [
      (3, DateTime.utc(2020, 12, 20)),
      (7, DateTime.utc(2021, 12, 20)),
      (5, DateTime.utc(2019, 12, 20)),
      (6, DateTime.utc(2020, 12, 20)),
      (10, DateTime.utc(2025, 12, 20)),
      (2, DateTime.utc(2016, 12, 20)),
    ];
    final favoritesContent = allContent.mapIndexed((i, content) {
      return Favorite(
        id: 1,
        userScore: favoriteData[i].$1,
        addedDate: favoriteData[i].$2,
        content: content,
      );
    }).toList();

    final filteredContent = favoritesContent.where((favorite) {
      if (params.searchQuery.isNotEmpty &&
          !favorite.content.title.toLowerCase().contains(params.searchQuery.toLowerCase())) {
        return false;
      }
      return true;
    }).toList();

    filteredContent.sort((a, b) {
      switch (params.sortBy) {
        case FavoritesSortBy.userRatingDesc:
          return b.userScore.compareTo(a.userScore);
        case FavoritesSortBy.userRatingAsc:
          return a.userScore.compareTo(b.userScore);
        case FavoritesSortBy.addedDateDesc:
          return b.addedDate.compareTo(a.addedDate);
        case FavoritesSortBy.addedDateAsc:
          return a.addedDate.compareTo(b.addedDate);
        case FavoritesSortBy.publicRatingDesc:
          return b.content.rating.compareTo(a.content.rating);
        case FavoritesSortBy.publicRatingAsc:
          return a.content.rating.compareTo(b.content.rating);
        case FavoritesSortBy.dateDesc:
          return b.content.year.compareTo(a.content.year);
        case FavoritesSortBy.dateAsc:
          return a.content.year.compareTo(b.content.year);
        case FavoritesSortBy.titleAsc:
          return a.content.title.compareTo(b.content.title);
        case FavoritesSortBy.titleDesc:
          return b.content.title.compareTo(a.content.title);
        default:
          return 0;
      }
    });
    final takenContent = filteredContent.skip(page * perPage).take(perPage).toList();
    return takenContent;
  }
}