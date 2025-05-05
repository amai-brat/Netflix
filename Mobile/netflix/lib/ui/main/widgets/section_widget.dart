import 'package:flutter/material.dart';
import 'package:netflix/domain/models/sections/section.dart';
import 'package:netflix/ui/core/widgets/content_card.dart';

class SectionWidget extends StatelessWidget {
  final Section section;

  const SectionWidget({super.key, required this.section});

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      height: 360,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Padding(
            padding: const EdgeInsets.symmetric(
              horizontal: 16.0,
              vertical: 8.0,
            ),
            child: Text(
              section.name,
              style: Theme.of(
                context,
              ).textTheme.headlineSmall?.copyWith(fontWeight: FontWeight.bold),
            ),
          ),
          Expanded(
            child: SingleChildScrollView(
              scrollDirection: Axis.horizontal,
              child: Row(
                children:
                    section.contents
                        .map(
                          (c) => SizedBox(
                            width: 200,
                            child: ContentCard(
                              id: c.id,
                              name: c.name,
                              posterUrl: c.posterUrl,
                            ),
                          ),
                        )
                        .toList(),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
