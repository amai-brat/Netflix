import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/components/favorite_search_bar.dart';
import 'package:netflix/ui/profile/tabs/favorites/widgets/sort_window.dart';
import 'package:netflix/utils/app_colors.dart';

class SearchSortRow extends StatelessWidget {
  const SearchSortRow({super.key});

  @override
  Widget build(BuildContext context) {
    final ctx = context.read<FavoriteBloc>();

    return Padding(
      padding: const EdgeInsets.symmetric(horizontal: 4.0),
      child: Row(
        children: [
          Expanded(
              child: FavoriteSearchBar()
          ),
          IconButton(
            icon: Icon(Icons.sort, color: AppColors.primaryRed, size: 42),
            onPressed:
                () => showDialog(
              context: context,
              builder: (_) => BlocProvider.value(value: ctx, child: SortWindow()),
            ),
            style: IconButton.styleFrom(
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(8)
              )
            ),
          ),
        ],
      ),
    );
  }
}
