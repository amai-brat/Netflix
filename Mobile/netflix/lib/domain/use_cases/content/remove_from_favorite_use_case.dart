import 'package:netflix/domain/repositories/favorite_repository.dart';
import 'package:netflix/utils/result.dart';

class RemoveFromFavoriteUseCase {
  final FavoriteRepository _favoriteRepository;

  RemoveFromFavoriteUseCase({required FavoriteRepository favoriteRepository})
      : _favoriteRepository = favoriteRepository;

  Future<Result<bool>> execute(int contentId) async {
    try {
      await _favoriteRepository.removeFromFavourites(contentId);
      return Result.ok(true);
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}