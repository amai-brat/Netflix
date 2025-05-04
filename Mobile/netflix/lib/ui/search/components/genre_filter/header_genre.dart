import 'package:flutter/material.dart';
import 'package:netflix/utils/app_colors.dart';

class HeaderGenre extends StatelessWidget{
  const HeaderGenre({super.key});

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Row(
            children: [
              TextButton.icon(
                label: Text('Жанры', style: TextStyle(color: AppColors.textWhite, fontSize: 20, fontWeight: FontWeight.bold)),
                icon: Icon(Icons.arrow_back, color: AppColors.primaryRed, size: 28),
                onPressed: () => Navigator.pop(context),
              ),
            ]
        ),
        Container(
          height: 2,
          color: AppColors.inputGrey,
        )
      ],
    );
  }
}