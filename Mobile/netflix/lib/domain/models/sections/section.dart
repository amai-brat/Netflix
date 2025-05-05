import 'package:netflix/domain/models/sections/section_content.dart';

class Section {
  final String name;
  final List<SectionContent> contents;

  Section({required this.name, required this.contents});
}
