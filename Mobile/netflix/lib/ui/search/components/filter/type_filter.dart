import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/ui/search/bloc/search_state.dart';
import 'package:netflix/utils/app_colors.dart';

class TypeFilter extends StatelessWidget {
  const TypeFilter({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Text('Тип', style: TextStyle(color: AppColors.textWhite, fontSize: 18, fontWeight: FontWeight.bold)),
        BlocBuilder<SearchBloc, SearchState>(
          builder: (context, state) {
            final ctx = context.read<SearchBloc>();
            final allTypes = state.availableTypes;
            final selectedTypes = state.filterParams.selectedTypes;
            final selectedTypesIds = selectedTypes.map((type) => type.id).toList();

            return ListView.builder(
              shrinkWrap: true,
              physics: ClampingScrollPhysics(),
              itemCount: allTypes.length,
              itemBuilder: (context, i) =>
                  CheckboxListTile(
                    checkColor: AppColors.textWhite,
                    title: Text(allTypes[i].name,
                        style: TextStyle(color: AppColors.textWhite, fontSize: 16)),
                    value: selectedTypesIds.contains(allTypes[i].id),
                    onChanged: (value) {
                      if (value == null) return;

                      final newTypes = value
                          ? [...selectedTypes, allTypes[i]]
                          : selectedTypes.where((sType) =>
                      sType.id != allTypes[i].id).toList();

                      ctx.add(
                          UpdateFilterParams((oldParams) =>
                              oldParams.copyWith(
                                  selectedTypes: (newTypes, false)
                              )));
                    },
                  ),
            );
          }
        )
      ],
    );
  }
}


