import 'package:collection/collection.dart';
import 'package:netflix/domain/models/content/age_ratings.dart';
import 'package:netflix/domain/models/content/budget.dart';
import 'package:netflix/domain/models/content/content.dart';
import 'package:netflix/domain/models/content/person_in_content.dart';
import 'package:netflix/domain/models/content/profession.dart';
import 'package:netflix/domain/models/content/ratings.dart';
import 'package:netflix/domain/models/content/trailer_info.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/domain/models/content/content_type.dart';
import 'package:netflix/domain/models/content/genre.dart';
import 'package:netflix/domain/models/sections/section.dart';
import 'package:netflix/domain/models/sections/section_content.dart';
import 'package:netflix/domain/repositories/content_repository.dart';
import 'package:netflix/domain/responses/sections_response.dart';
import 'package:netflix/utils/result.dart';

class ContentRepositoryMock extends ContentRepository {
  static const description = """
Сотрудник страховой компании страдает хронической бессонницей и отчаянно пытается вырваться из мучительно скучной жизни. Однажды в очередной командировке он встречает некоего Тайлера Дёрдена — харизматического торговца мылом с извращенной философией. Тайлер уверен, что самосовершенствование — удел слабых, а единственное, ради чего стоит жить, — саморазрушение.
Проходит немного времени, и вот уже новые друзья лупят друг друга почем зря на стоянке перед баром, и очищающий мордобой доставляет им высшее блаженство. Приобщая других мужчин к простым радостям физической жестокости, они основывают тайный Бойцовский клуб, который начинает пользоваться невероятной популярностью.
""";

  static const allContent = [
    Content(
      id: 1,
      title: 'Aboba',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 7.5,
      year: 2020,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 1, name: 'Фильм'),
      country: 'США',
      slogan: 'Em',
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [
        PersonInContent(
          id: 1,
          contentId: 1,
          name: 'Ильюка Мэддисон',
          profession: Profession(id: 1, professionName: 'Клоун'),
        ),
      ],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
      description: description,
    ),
    Content(
      id: 2,
      title: 'Biba',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5,
      year: 2024,
      genres: [Genre(id: 2, name: 'Драма'), Genre(id: 3, name: 'Комедия')],
      type: ContentType(id: 3, name: 'Мультфильм'),
      country: 'США',
      slogan: 'Em',
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
      description: description,
    ),
    Content(
      id: 3,
      title: 'Boba',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 1,
      year: 2000,
      genres: [Genre(id: 10, name: 'Фантастика'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 4,
      title: 'Tralalelo',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 9,
      year: 2014,
      genres: [
        Genre(id: 6, name: 'Повседневность'),
        Genre(id: 4, name: 'Триллер'),
      ],
      type: ContentType(id: 1, name: 'Фильм'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 5,
      title: 'Bambini',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.2,
      year: 2019,
      genres: [
        Genre(id: 6, name: 'Повседневность'),
        Genre(id: 9, name: 'Трагедия'),
      ],
      type: ContentType(id: 3, name: 'Мультфильм'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 6,
      title: 'Gussini',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.9,
      year: 2014,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 7,
      title: 'Test1',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.9,
      year: 2014,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 8,
      title: 'Test2',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.9,
      year: 2014,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 9,
      title: 'Test3',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.9,
      year: 2014,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 10,
      title: 'Test4',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.9,
      year: 2014,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 11,
      title: 'Test5',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.9,
      year: 2014,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
    Content(
      id: 12,
      title: 'Test6',
      posterUrl:
          'https://avatars.mds.yandex.net/get-entity_search/2362199/1143543517/S600xU_2x',
      rating: 5.9,
      year: 2014,
      genres: [Genre(id: 1, name: 'Боевик'), Genre(id: 2, name: 'Драма')],
      type: ContentType(id: 2, name: 'Сериал'),
      country: 'США',
      slogan: 'Em',
      description: description,
      ageRatings: AgeRatings(age: 18, ageMpaa: 'R'),
      budget: Budget(budgetCurrencyName: '\$', budgetValue: 228000),
      personsInContent: [],
      ratings: Ratings(imdbRating: 9.8, kinopoiskRating: 7.4, localRating: 5),
      trailerInfo: TrailerInfo(
        name: 'SHTO',
        url: 'https://www.youtube.com/watch?v=dQw4w9WgXcQ',
      ),
    ),
  ];

  @override
  Future<List<Content>> getContent(
    ContentFilterParams params,
    int page,
    int perPage,
  ) async {
    await Future.delayed(Duration(seconds: 1));

    final filteredContent =
        allContent.where((movie) {
          if (params.searchQuery.isNotEmpty &&
              !movie.title.toLowerCase().contains(
                params.searchQuery.toLowerCase(),
              )) {
            return false;
          }
          if (params.selectedGenres.isNotEmpty &&
              !params.selectedGenres.every(
                (genre) =>
                    movie.genres.any((movieGenre) => movieGenre.id == genre.id),
              )) {
            return false;
          }
          if (params.selectedTypes.isNotEmpty &&
              !params.selectedTypes.any((type) => type.id == movie.type.id)) {
            return false;
          }
          if (params.country != null &&
              movie.country.toLowerCase() != params.country!.toLowerCase()) {
            return false;
          }
          if (params.yearFrom != null && movie.year < params.yearFrom!) {
            return false;
          }
          if (params.yearTo != null && movie.year > params.yearTo!) {
            return false;
          }
          if (params.ratingFrom != null && movie.rating < params.ratingFrom!) {
            return false;
          }
          if (params.ratingTo != null && movie.rating > params.ratingTo!) {
            return false;
          }
          return true;
        }).toList();

    filteredContent.sort((a, b) {
      switch (params.sortBy) {
        case SortBy.ratingDesc:
          return b.rating.compareTo(a.rating);
        case SortBy.ratingAsc:
          return a.rating.compareTo(b.rating);
        case SortBy.dateDesc:
          return b.year.compareTo(a.year);
        case SortBy.dateAsc:
          return a.year.compareTo(b.year);
        case SortBy.titleAsc:
          return a.title.compareTo(b.title);
        case SortBy.titleDesc:
          return b.title.compareTo(a.title);
        default:
          return 0;
      }
    });
    final takenContent =
        filteredContent.skip(page * perPage).take(perPage).toList();
    return takenContent;
  }

  @override
  Future<Result<Content>> getContentById({required int contentId}) async {
    await Future.delayed(Duration(milliseconds: 500));

    final content = allContent.firstWhereOrNull((c) => c.id == contentId);
    if (content == null) {
      return Result.error('Контнет не найден');
    }

    return Result.ok(content);
  }

  @override
  Future<Result<SectionsResponse>> getSections() async {
    final sectionNames = ['Новинки', 'Классика', 'Аниме'];
    return Result.ok(
      SectionsResponse(
        data:
            sectionNames
                .map(
                  (name) => Section(
                    name: name,
                    contents:
                        allContent
                            .map((c) => SectionContent.fromContent(c))
                            .toList(),
                  ),
                )
                .toList(),
      ),
    );
  }
}
