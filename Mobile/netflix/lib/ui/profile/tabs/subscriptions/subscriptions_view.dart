import 'package:flutter/material.dart';

class SubscriptionsView extends StatefulWidget {
  const SubscriptionsView({super.key});

  @override
  State<StatefulWidget> createState() => _SubscriptionsViewState();
}

class _SubscriptionsViewState extends State<SubscriptionsView> {
  final check = UniqueKey();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Text('$check'),
    );
  }
}