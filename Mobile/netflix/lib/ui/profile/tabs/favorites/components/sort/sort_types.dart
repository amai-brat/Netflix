import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/favorite_filter_params.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_event.dart';
import 'package:netflix/utils/app_colors.dart';

class SortTypes extends StatelessWidget {
  const SortTypes({super.key});

  @override
  Widget build(BuildContext context) {
    final ctx = context.read<FavoriteBloc>();
    return ListView.builder(
      shrinkWrap: true,
      physics: ClampingScrollPhysics(),
      itemCount: FavoritesSortBy.values.length,
      itemBuilder: (context, i) => RadioListTile<FavoritesSortBy>(
        title: Text(
          getSortTitle(FavoritesSortBy.values[i]),
          style: const TextStyle(color: AppColors.textWhite, fontSize: 14),
        ),
        value: FavoritesSortBy.values[i],
        groupValue: ctx.state.filterParams.sortBy,
        onChanged: (value) {
          if(value == null) return;
          ctx.add(SortChanged(value));
          Navigator.pop(context);
        },
      ),
    );
  }

  static String getSortTitle(FavoritesSortBy sort) {
    switch (sort) {
      case FavoritesSortBy.addedDateDesc:
        return 'Дата добавления (новые → старые)';
      case FavoritesSortBy.addedDateAsc:
        return 'Дата добавления (старые → новые)';
      case FavoritesSortBy.userRatingDesc:
        return 'Моя оценка (высокий → низкий)';
      case FavoritesSortBy.userRatingAsc:
        return 'Моя оценка (низкий → высокий)';
      case FavoritesSortBy.publicRatingDesc:
        return 'Рейтинг (высокий → низкий)';
      case FavoritesSortBy.publicRatingAsc:
        return 'Рейтинг (низкий → высокий)';
      case FavoritesSortBy.dateDesc:
        return 'Дата выхода (новые → старые)';
      case FavoritesSortBy.dateAsc:
        return 'Дата выхода (старые → новые)';
      case FavoritesSortBy.titleAsc:
        return 'Название (А → Я)';
      case FavoritesSortBy.titleDesc:
        return 'Название (Я → А)';
    }
  }
}