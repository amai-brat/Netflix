import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/use_cases/content/get_favorite_by_filter_use_case.dart';
import 'package:netflix/domain/use_cases/content/remove_from_favorite_use_case.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_event.dart';
import 'package:netflix/ui/profile/tabs/favorites/components/content/favorite_list.dart';
import 'package:netflix/ui/profile/tabs/favorites/components/search_sort_row.dart';
import 'package:netflix/utils/app_colors.dart';
import 'package:netflix/utils/di.dart';

class FavoritesView extends StatelessWidget {
  const FavoritesView({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => FavoriteBloc(
        getFavoriteByFilterUseCase: locator<GetFavoriteByFilterUseCase>(),
        removeFromFavoriteUseCase: locator<RemoveFromFavoriteUseCase>()
      )..add(LoadInitialData()),
      child: Scaffold(
          appBar: AppBar(
            title: const Text('Избранное'),
            backgroundColor: AppColors.inputGrey,
          ),
          backgroundColor: AppColors.backgroundBlack,
          body: Column(
            children: [
              SearchSortRow(),
              Expanded(child: FavoriteList()),
            ],
          )
      ),
    );
  }
}
