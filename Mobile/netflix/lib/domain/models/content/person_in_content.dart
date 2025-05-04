import 'package:netflix/domain/models/content/profession.dart';

class PersonInContent {
  final int id;
  final int contentId;
  final String name;
  final Profession profession;

  const PersonInContent({required this.id, required this.contentId, required this.name, required this.profession});
}