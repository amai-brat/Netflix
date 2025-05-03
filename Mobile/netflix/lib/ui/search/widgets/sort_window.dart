import 'package:flutter/material.dart';
import 'package:netflix/ui/search/components/sort/header_sort.dart';
import 'package:netflix/ui/search/components/sort/sort_actions.dart';
import 'package:netflix/ui/search/components/sort/sort_types.dart';
import 'package:netflix/utils/app_colors.dart';

class SortWindow extends StatelessWidget {
  const SortWindow({super.key});

  @override
  Widget build(BuildContext context) {
    return Dialog(
      backgroundColor: AppColors.backgroundBlack,
      child: Padding(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            HeaderSort(),
            SortTypes(),
            SortActions()
          ],
        ),
      ),
    );
  }
}
