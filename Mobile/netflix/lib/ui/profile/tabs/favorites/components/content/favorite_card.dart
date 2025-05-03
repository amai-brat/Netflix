import 'package:flutter/material.dart';
import 'package:netflix/domain/models/favorite.dart';
import 'package:netflix/utils/app_colors.dart';

class FavoriteCard extends StatelessWidget {
  final Favorite favorite;

  const FavoriteCard({super.key, required this.favorite});

  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.grey[900],
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(8),
      ),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          ClipRRect(
            borderRadius: const BorderRadius.horizontal(
              left: Radius.circular(8),
            ),
            child: Container(
              width: 140,
              height: 200,
              decoration: BoxDecoration(
                borderRadius: const BorderRadius.horizontal(
                  left: Radius.circular(8),
                ),
              ),
              child: Image.network(
                favorite.content.posterUrl,
                fit: BoxFit.cover,
              ),
            ),
          ),
          Expanded(
            child: Padding(
              padding: const EdgeInsets.all(12.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    favorite.content.title,
                    style: const TextStyle(
                      color: AppColors.textWhite,
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                    maxLines: 2,
                    overflow: TextOverflow.ellipsis,
                  ),
                  Text(
                    'Моя оценка: ${favorite.userScore}',
                    style: TextStyle(
                      color: AppColors.textWhite,
                      fontSize: 16,
                      fontWeight: FontWeight.w300
                    ),
                  ),
                  Text(
                    'Добавлено: ${_formatDate(favorite.addedDate)}',
                    style: TextStyle(
                      color: AppColors.textWhite,
                      fontSize: 16,
                      fontWeight: FontWeight.w300
                    ),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }

  String _formatDate(DateTime date) {
    final day = date.day.toString().padLeft(2, '0');
    final month = date.month.toString().padLeft(2, '0');
    return '$day.$month.${date.year}';
  }
}