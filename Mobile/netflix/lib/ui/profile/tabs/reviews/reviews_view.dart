import 'package:flutter/material.dart';

class ReviewsView extends StatefulWidget {
  const ReviewsView({super.key});

  @override
  State<StatefulWidget> createState() => _ReviewsViewState();
}

class _ReviewsViewState extends State<ReviewsView> {
  final check = UniqueKey();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Рецензии')),
      body: Center(child: Text('$check')),
    );
  }
}