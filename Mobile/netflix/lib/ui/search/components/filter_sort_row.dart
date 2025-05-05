import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/content_filter_params.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/ui/search/components/sort/sort_types.dart';
import 'package:netflix/ui/search/widgets/filter_window.dart';
import 'package:netflix/ui/search/widgets/sort_window.dart';
import 'package:netflix/utils/app_colors.dart';

class FilterSortRow extends StatelessWidget{
  const FilterSortRow({super.key});

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 0.0),
      child: BlocBuilder<SearchBloc, SearchState>(
        builder: (context, state) {
          final ctx = context.read<SearchBloc>();
          return Row(
            children: [
              Expanded(
                child: TextButton.icon(
                  icon: Icon(Icons.sort, color: AppColors.primaryRed, size: 28),
                  label: Text(
                    getSortLabel(state.filterParams.sortBy),
                    style: TextStyle(
                      color: AppColors.textWhite,
                      fontWeight: FontWeight.bold,
                      fontSize: getSortLabel(state.filterParams.sortBy).length > 12 ? 14 : 18),
                  ),
                  onPressed: () => showDialog(
                    context: context,
                    builder: (_) => BlocProvider.value(
                      value: ctx,
                      child: SortWindow(),
                    ),
                  ),
                ),
              ),
              Expanded(
                child: TextButton.icon(
                  icon: Icon(Icons.filter_list, color: AppColors.primaryRed, size: 28),
                  label: Text(
                    'Фильтры',
                    style: TextStyle(
                      color: AppColors.textWhite,
                      fontWeight: FontWeight.bold,
                      fontSize: 18
                    ),
                  ),
                  onPressed: () => showDialog(
                    context: context,
                    builder: (_) => BlocProvider.value(
                      value: ctx,
                      child: FilterWindow(),
                    ),
                  ),
                ),
              )
            ],
          );
        }
      )
    );
  }

  String getSortLabel(SortBy? sort) {
    if (sort == null) return 'Сортировка';
    return SortTypes.getSortTitle(sort);
  }
}
