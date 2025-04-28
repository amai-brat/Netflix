import 'package:flutter/material.dart';

class PersonalInfoView extends StatefulWidget {
  const PersonalInfoView({super.key});

  @override
  State<StatefulWidget> createState() => _PersonalInfoViewState();
}

class _PersonalInfoViewState extends State<PersonalInfoView> {
  final check = UniqueKey();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text('$check'),
    );
  }
}