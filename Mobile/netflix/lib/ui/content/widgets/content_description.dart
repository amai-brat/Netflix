import 'package:flutter/material.dart';

class ContentDescription extends StatelessWidget {
  final String description;

  const ContentDescription({super.key, required this.description});

  @override
  Widget build(BuildContext context) {
    return Container(
      margin: EdgeInsets.symmetric(vertical: 2, horizontal: 10),
      padding: EdgeInsets.symmetric(horizontal: 20, vertical: 10),
      decoration: BoxDecoration(color: Colors.grey[900], borderRadius: BorderRadius.vertical(top: Radius.circular(10))),
      child: Text(description, style: Theme.of(context).textTheme.bodyLarge, textAlign: TextAlign.justify,),
    );
  }
}