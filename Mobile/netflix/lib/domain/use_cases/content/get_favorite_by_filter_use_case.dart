import 'package:netflix/domain/models/favorite.dart';
import 'package:netflix/domain/models/favorite_filter_params.dart';
import 'package:netflix/domain/repositories/favorite_repository.dart';
import 'package:netflix/utils/result.dart';

class GetFavoriteByFilterUseCase {
  final FavoriteRepository _favoriteRepository;

  GetFavoriteByFilterUseCase({required FavoriteRepository favoriteRepository})
      : _favoriteRepository = favoriteRepository;

  Future<Result<List<Favorite>>> execute(FavoriteFilterParams params, int page, int perPage) async {
    try {
      return Result.ok(await _favoriteRepository.getFavorites(params, page, perPage));
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}