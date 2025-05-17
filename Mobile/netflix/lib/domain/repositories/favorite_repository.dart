import 'package:netflix/domain/models/favorite.dart';
import 'package:netflix/domain/models/favorite_filter_params.dart';

abstract class FavoriteRepository {
  Future<List<Favorite>> getFavorites(FavoriteFilterParams params, int page, int perPage);
  Future<void> removeFromFavourites(int contentId);
}