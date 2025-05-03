import 'package:flutter/material.dart';
import 'package:netflix/domain/models/content.dart';
import 'package:netflix/utils/app_colors.dart';

class ContentCard extends StatelessWidget {
  final Content content;

  const ContentCard({super.key, required this.content});

  @override
  Widget build(BuildContext context) {
    return Card(
      color: Colors.grey[900],
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.circular(8),
      ),
      child: Column(
        children: [
          Expanded(
            child: ClipRRect(
              borderRadius: BorderRadius.all(Radius.circular(8)),
              child: Container(
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.all(Radius.circular(8)),
                ),
                child: Image.network(
                  content.posterUrl,
                  fit: BoxFit.cover,
                  width: double.infinity,
                ),
              ),
            ),
          ),
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: Text(
              content.title,
              style: TextStyle(
                color: AppColors.textWhite,
                fontSize: 16,
                fontWeight: FontWeight.bold
              ),
              maxLines: 1,
              overflow: TextOverflow.ellipsis,
            ),
          ),
        ],
      ),
    );
  }
}