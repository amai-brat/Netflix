import 'package:flutter/material.dart';
import 'package:netflix/utils/app_colors.dart';

class FilterHeader extends StatelessWidget {
  const FilterHeader({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
        children: [
          Row(
            children: [
              TextButton.icon(
                icon: Icon(Icons.arrow_back, color: AppColors.primaryRed, size: 24),
                onPressed: () => Navigator.pop(context),
                label: Text('Фильтры', style: TextStyle(color: AppColors.textWhite, fontSize: 20, fontWeight: FontWeight.bold)),
              ),
            ],
          ),
          Container(
            height: 2,
            color: AppColors.inputGrey,
          ),
        ]
      );
  }
}