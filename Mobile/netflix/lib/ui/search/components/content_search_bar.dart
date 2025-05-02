import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/utils/app_colors.dart';

class ContentSearchBar extends StatefulWidget {
  const ContentSearchBar({super.key});

  @override
  State<ContentSearchBar> createState() => _ContentSearchBarState();
}

class _ContentSearchBarState extends State<ContentSearchBar> {
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
      child: BlocBuilder<SearchBloc, SearchState>(
        builder: (context, state) {
          final ctx = context.read<SearchBloc>();
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