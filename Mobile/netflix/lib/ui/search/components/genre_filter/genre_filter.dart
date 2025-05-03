import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/widgets/genre_window.dart';
import 'package:netflix/utils/app_colors.dart';

class GenreFilter extends StatelessWidget {
  const GenreFilter({super.key,});

  @override
  Widget build(BuildContext context) {
    final ctx = context.read<SearchBloc>();
    final selectedGenres = ctx.state.filterParams.selectedGenres;
    final genreNames = selectedGenres.take(2).map((g) => g.name).toList();
    final label =
        genreNames.join(', ') +
        (selectedGenres.length > 2 ? '...' : '') +
        (selectedGenres.isEmpty ? 'Не выбрано' : '');

    return Row(
      crossAxisAlignment: CrossAxisAlignment.center,
      mainAxisAlignment: MainAxisAlignment.spaceBetween,
      children: [
        Text('Жанры', style: TextStyle(color: AppColors.textWhite, fontSize: 18, fontWeight: FontWeight.bold)),
        TextButton.icon(
          icon: Text(label, style: TextStyle(fontSize: 16)),
          onPressed: () => showDialog(
            context: context,
            builder: (_) => BlocProvider.value(
              value: ctx,
              child: GenreWindow(),
            ),
          ),
          label: Icon(Icons.arrow_forward_ios, color: AppColors.primaryRed, size: 18),
        ),
      ],
    );
  }
}
