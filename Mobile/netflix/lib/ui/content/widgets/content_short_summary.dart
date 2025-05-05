import 'package:flutter/material.dart';
import 'package:netflix/domain/models/content/content.dart';

class ContentShortSummary extends StatelessWidget {
  final Content content;

  const ContentShortSummary({super.key, required this.content});

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: EdgeInsets.all(10),
      decoration: BoxDecoration(
        color: Colors.grey[900],
        borderRadius: BorderRadius.all(Radius.circular(10)),
      ),
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          Image.network(
            content.posterUrl,
            fit: BoxFit.cover,
            height: MediaQuery.of(context).size.height * 0.4,
          ),
          const SizedBox(height: 20),
          Text(
            content.title,
            style: Theme.of(
              context,
            ).textTheme.headlineLarge?.copyWith(fontWeight: FontWeight.bold),
          ),
          Text(content.slogan, style: Theme.of(context).textTheme.labelLarge),
          const SizedBox(height: 10),
          Text(
            '${content.year}, ${content.genres.map((g) => g.name).join(', ')}',
          ),
          Text('${content.country}, ${content.ageRatings.age}+'),
        ],
      ),
    );
  }
}
