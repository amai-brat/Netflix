import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/utils/app_colors.dart';

class ContentSearchBar extends StatelessWidget {
  const ContentSearchBar({super.key});

  @override
  Widget build(BuildContext context) {
    final ctx = context.read<SearchBloc>();

    return Padding(
      padding: EdgeInsets.symmetric(horizontal: 12.0, vertical: 12.0),
      child: TextField(
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
      ),
    );
  }
}