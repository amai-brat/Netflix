import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/utils/app_colors.dart';

class FilterActions extends StatelessWidget {
  const FilterActions({super.key});

  @override
  Widget build(BuildContext context) {
    final ctx = context.read<SearchBloc>();
    return Column(
      spacing: 10,
      children: [
        Container(
          height: 2,
          color: AppColors.inputGrey,
        ),
        Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Expanded(
              child: Padding(
                padding: const EdgeInsets.only(right: 8),
                child: OutlinedButton(
                  onPressed: () {
                    ctx.add(ResetFilters());
                  },
                  style: OutlinedButton.styleFrom(
                    side: BorderSide(
                      color: AppColors.primaryRed,
                      width: 1.5,
                    ),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(8),
                    ),
                    padding: EdgeInsets.symmetric(vertical: 12),
                  ),
                  child: Text(
                    ' Сбросить ',
                    style: TextStyle(
                      color: AppColors.primaryRed,
                      fontSize: 16,
                    ),
                  ),
                ),
              ),
            ),
            Expanded(
              child: ElevatedButton(
                style: ElevatedButton.styleFrom(
                  backgroundColor: AppColors.primaryRed,
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(8),
                  ),
                  padding: EdgeInsets.symmetric(vertical: 10),
                ),
                onPressed: () {
                  ctx.add(ApplyFilters(ctx.state.filterParams));
                  Navigator.pop(context);
                },
                child: Text(
                  'Применить',
                  style: TextStyle(
                    fontSize: 18,
                  ),
                ),
              ),
            ),
          ],
        )
      ],
    );
  }
}