import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/utils/app_colors.dart';

class GenreTypes extends StatelessWidget{
  const GenreTypes({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SearchBloc, SearchState>(
        builder: (context, state)
        {
          final ctx = context.read<SearchBloc>();
          final allGenres = state.availableGenres;
          final selectedGenres = state.filterParams.selectedGenres;
          final selectedGenresIds = selectedGenres.map((genre) => genre.id).toList();

          return ListView.builder(
            shrinkWrap: true,
            physics: ClampingScrollPhysics(),
            itemCount: allGenres.length,
            itemBuilder: (context, i) => CheckboxListTile(
              checkColor: AppColors.textWhite,
              title: Text(allGenres[i].name, style: TextStyle(color: AppColors.textWhite, fontSize: 16)),
              value: selectedGenresIds.contains(allGenres[i].id),
              onChanged: (value) {
                if (value == null) return;

                final newGenres = value
                    ? [...selectedGenres, allGenres[i]]
                    : selectedGenres.where((sType) => sType.id != allGenres[i].id).toList();

                ctx.add(UpdateFilterParams((oldParams) => oldParams.copyWith(
                    selectedGenres: (newGenres, false)
                )));
              },
            ),
          );
        }
    );
  }

}