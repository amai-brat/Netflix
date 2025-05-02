import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_bloc.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_event.dart';
import 'package:netflix/ui/profile/tabs/favorites/bloc/favorites_state.dart';
import 'package:netflix/utils/app_colors.dart';

class FavoriteSearchBar extends StatefulWidget {
  const FavoriteSearchBar({super.key});

  @override
  State<FavoriteSearchBar> createState() => _FavoriteSearchBarState();
}

class _FavoriteSearchBarState extends State<FavoriteSearchBar> {
  late TextEditingController _searchBarController;

  @override
  void initState() {
    super.initState();
    _searchBarController = TextEditingController();
  }

  @override
  void dispose() {
    _searchBarController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 12.0, vertical: 12.0),
      child: BlocBuilder<FavoriteBloc, FavoriteState>(
        builder: (context, state) {
          final ctx = context.read<FavoriteBloc>();
          _searchBarController.text = state.filterParams.searchQuery;

          return TextField(
            controller: _searchBarController,
            style: TextStyle(color: AppColors.textWhite),
            decoration: InputDecoration(
              prefixIcon: Icon(Icons.search),
              filled: true,
              fillColor: Colors.grey[800],
              hintText: 'Название фильма...',
              focusedBorder: const OutlineInputBorder(
                  borderSide: BorderSide(
                    color: AppColors.textWhite,
                  ),
                  borderRadius: BorderRadius.all(Radius.circular(8))
              ),
              border: const OutlineInputBorder(
                  borderSide: BorderSide(
                    color: AppColors.textWhite,
                  ),
                  borderRadius: BorderRadius.all(Radius.circular(8))
              ),
            ),
            onChanged: (query) {
              ctx.add(SearchQueryChanged(query));
            },
          );
        }
      )
    );
  }
}