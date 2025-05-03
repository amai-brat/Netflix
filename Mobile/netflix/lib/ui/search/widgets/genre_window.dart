import 'package:flutter/material.dart';
import 'package:netflix/ui/search/components/genre_filter/genre_types.dart';
import 'package:netflix/ui/search/components/genre_filter/header_genre.dart';
import 'package:netflix/utils/app_colors.dart';

class GenreWindow extends StatelessWidget {
  const GenreWindow({super.key,});

  @override
  Widget build(BuildContext context) {
    final mediaQuery = MediaQuery.of(context);

    return Dialog(
      insetPadding: EdgeInsets.zero,
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.zero,
      ),
      backgroundColor: AppColors.backgroundBlack,
      child: SizedBox(
        width: mediaQuery.size.width,
        height: mediaQuery.size.height,
        child: SingleChildScrollView(
          padding: EdgeInsets.all(20.0),
          child: Column(
            children: [
              HeaderGenre(),
              GenreTypes()
            ]
          )
        )
      )
    );
  }
}