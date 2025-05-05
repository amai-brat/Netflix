import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/utils/app_colors.dart';

class SortTypes extends StatelessWidget {
  const SortTypes({super.key});

  @override
  Widget build(BuildContext context) {
    final ctx = context.read<SearchBloc>();
    return ListView.builder(
      shrinkWrap: true,
      physics: ClampingScrollPhysics(),
      itemCount: SortBy.values.length,
      itemBuilder: (context, i) => RadioListTile<SortBy>(
        title: Text(
          getSortTitle(SortBy.values[i]),
          style: const TextStyle(color: AppColors.textWhite, fontSize: 16),
        ),
        value: SortBy.values[i],
        groupValue: ctx.state.filterParams.sortBy,
        onChanged: (value) {
          if(value == null) return;
          ctx.add(SortChanged(value));
          Navigator.pop(context);
        },
      ),
    );
  }

  static String getSortTitle(SortBy sort) {
    switch (sort) {
      case SortBy.ratingDesc:
        return 'Рейтинг (высокий → низкий)';
      case SortBy.ratingAsc:
        return 'Рейтинг (низкий → высокий)';
      case SortBy.dateDesc:
        return 'Дата выхода (новые → старые)';
      case SortBy.dateAsc:
        return 'Дата выхода (старые → новые)';
      case SortBy.titleAsc:
        return 'Название (А → Я)';
      case SortBy.titleDesc:
        return 'Название (Я → А)';
    }
  }
}