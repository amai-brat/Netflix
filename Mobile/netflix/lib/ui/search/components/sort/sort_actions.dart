import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/search/bloc/search_bloc.dart';
import 'package:netflix/ui/search/bloc/search_event.dart';
import 'package:netflix/utils/app_colors.dart';

class SortActions extends StatelessWidget{
  const SortActions({super.key});

  @override
  Widget build(BuildContext context) {
    final ctx = context.read<SearchBloc>();

    return Column(
      spacing: 4,
      children: [
        Container(
          height: 2,
          color: AppColors.inputGrey,
        ),
        Row(
          children: [
            Expanded(
              child: OutlinedButton(
                onPressed: () {
                  ctx.add(ResetSort());
                  Navigator.pop(context);
                },
                style: OutlinedButton.styleFrom(
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(8),
                  ),
                  side: BorderSide(
                    color: AppColors.primaryRed,
                    width: 1.5,
                  ),
                ),
                child: Text(
                  'Сбросить',
                  style: TextStyle(color: AppColors.primaryRed, fontSize: 18),
                ),
              ),
            )
          ],
        )
      ],
    );
  }
}