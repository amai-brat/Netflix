import 'package:flutter/material.dart';
import 'package:netflix/domain/models/content/ratings.dart';
import 'package:netflix/ui/content/widgets/content_section.dart';

class ContentRatings extends StatelessWidget {
  final Ratings ratings;

  const ContentRatings({super.key, required this.ratings});

  static const double logoSize = 40;

  static const logos = {
    'kinopoisk':
        'https://linku.su/uploads/block_thumbnail_images/f1810afecd5e6e610c4ceebef46ddd97.png',
    'imdb':
        'https://images.squarespace-cdn.com/content/v1/5a106bc6d7bdcee10244c71d/1613964260668-6PQV7FXARATH0DPSJT5E/1920px-IMDB_Logo_2016.svg.png',
    'local':
        'https://mir-s3-cdn-cf.behance.net/projects/max_808/787aa585172975.Y3JvcCwxMDIyLDgwMCw4OCww.jpeg',
  };

  Widget _buildRatingRow(Image logo, double ratingValue) {
    return Row(children: [logo, Text(': $ratingValue')]);
  }

  @override
  Widget build(BuildContext context) {
    // return Container(
    //   width: double.infinity,
    //   padding: EdgeInsets.symmetric(horizontal: 20, vertical: 10),
    //   decoration: BoxDecoration(color: Colors.grey[900]),
    //   child: Column(
    //     crossAxisAlignment: CrossAxisAlignment.start,
    //     children: [
    //       Text(
    //         'Рейтинги',
    //         style: Theme.of(
    //           context,
    //         ).textTheme.headlineMedium?.copyWith(fontWeight: FontWeight.bold),
    //         textAlign: TextAlign.left,
    //       ),
    //       const SizedBox(height: 10),
    //       _buildRatingRow(
    //         Image.network(logos['kinopoisk']!, height: logoSize, width: logoSize,),
    //         ratings.kinopoiskRating,
    //       ),
    //       _buildRatingRow(
    //         Image.network(logos['imdb']!, width: logoSize, height: logoSize),
    //         ratings.imdbRating,
    //       ),
    //       _buildRatingRow(
    //         Image.network(logos['local']!, width: logoSize, height: logoSize),
    //         ratings.localRating,
    //       ),
    //     ],
    //   ),
    // );
    return ContentSection(
      title: 'Рейтинги',
      children: [
        _buildRatingRow(
          Image.network(logos['kinopoisk']!, height: logoSize, width: logoSize),
          ratings.kinopoiskRating,
        ),
        _buildRatingRow(
          Image.network(logos['imdb']!, width: logoSize, height: logoSize),
          ratings.imdbRating,
        ),
        _buildRatingRow(
          Image.network(logos['local']!, width: logoSize, height: logoSize),
          ratings.localRating,
        ),
      ],
    );
  }
}
