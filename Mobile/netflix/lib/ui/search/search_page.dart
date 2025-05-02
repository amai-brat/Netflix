import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/use_cases/get_all_content_types_use_case.dart';
import 'package:netflix/domain/use_cases/get_all_genres_use_case.dart';
import 'package:netflix/domain/use_cases/get_content_by_filter_use_case.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/components/content/content_grid.dart';
import 'package:netflix/ui/search/components/content_search_bar.dart';
import 'package:netflix/ui/search/components/filter_sort_row.dart';
import 'package:netflix/utils/app_colors.dart';
import 'package:netflix/utils/di.dart';

class SearchPage extends StatelessWidget {
  const SearchPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => SearchBloc(
             getContentByFilterUseCase: locator<GetContentByFilterUseCase>(),
             getAllGenresUseCase: locator<GetAllGenresUseCase>(),
             getAllContentTypesUseCase: locator<GetAllContentTypesUseCase>()
         )..add(LoadInitialData()),
      child: Scaffold(
        appBar: AppBar(
          title: FilterSortRow(),
          backgroundColor: AppColors.inputGrey,
        ),
        backgroundColor: AppColors.backgroundBlack,
        body: Column(
          children: [
            ContentSearchBar(),
            Expanded(child: ContentGrid()),
          ],
        )
      ),
    );
  }
}