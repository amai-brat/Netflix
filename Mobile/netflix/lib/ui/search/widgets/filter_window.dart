import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/ui/search/components/filter/country_filter.dart';
import 'package:netflix/ui/search/components/filter/filter_actions.dart';
import 'package:netflix/ui/search/components/filter/header_filter.dart';
import 'package:netflix/ui/search/components/filter/rating_filter.dart';
import 'package:netflix/ui/search/components/filter/type_filter.dart';
import 'package:netflix/ui/search/components/filter/year_filter.dart';
import 'package:netflix/ui/search/components/genre_filter/genre_filter.dart';
import 'package:netflix/utils/app_colors.dart';

class FilterWindow extends StatelessWidget {
  const FilterWindow({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<SearchBloc, SearchState>(
      builder: (context, state) {
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
                spacing: 10,
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  FilterHeader(),
                  GenreFilter(),
                  TypeFilter(),
                  CountryFilter(),
                  YearFilter(),
                  RatingFilter(),
                  FilterActions()
                ],
              ),
            )
          ),
        );
      },
    );
  }
}