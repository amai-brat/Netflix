import 'package:netflix/domain/models/content/content.dart';

class SectionContent {
  final int id;
  final String name;
  final String posterUrl;

  SectionContent({
    required this.id,
    required this.name,
    required this.posterUrl,
  });

  factory SectionContent.fromContent(Content content) {
    return SectionContent(
      id: content.id,
      name: content.title,
      posterUrl: content.posterUrl,
    );
  }
}
