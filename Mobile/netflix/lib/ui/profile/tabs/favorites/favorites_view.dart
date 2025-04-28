import 'package:flutter/material.dart';

class FavoritesView extends StatefulWidget {
  const FavoritesView({super.key});

  @override
  State<StatefulWidget> createState() => _FavoritesViewState();
}

class _FavoritesViewState extends State<FavoritesView> {
  final check = UniqueKey();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text('$check'),
    );
  }
}