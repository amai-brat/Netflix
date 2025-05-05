import 'package:flutter/material.dart';
import 'package:netflix/domain/models/content/person_in_content.dart';
import 'package:netflix/ui/content/widgets/content_section.dart';

class ContentPersons extends StatelessWidget {
  final List<PersonInContent> personsInContent;

  const ContentPersons({super.key, required this.personsInContent});

  @override
  Widget build(BuildContext context) {
    if (personsInContent.isEmpty) {
      return Container();
    }

    return ContentSection(
      title: 'Участвовали',
      children:
          personsInContent
              .map((person) => Text('${person.name}: ${person.profession.professionName}', style: Theme.of(context).textTheme.bodyLarge,))
              .toList(),
    );
  }
}
