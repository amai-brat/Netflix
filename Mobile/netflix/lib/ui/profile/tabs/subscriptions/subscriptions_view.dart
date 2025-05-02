import 'package:flutter/material.dart';
import 'package:go_router/go_router.dart';
import 'package:netflix/utils/routes.dart';

class SubscriptionsView extends StatefulWidget {
  const SubscriptionsView({super.key});

  @override
  State<StatefulWidget> createState() => _SubscriptionsViewState();
}

class _SubscriptionsViewState extends State<SubscriptionsView> {
  final check = UniqueKey();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Мои подписки')),
      body: Center(child: Column(
        children: [
          Text('$check'),
          ElevatedButton(onPressed: () {
            context.push(Routes.subscriptions);
          }, child: Text('Купить подписку'))
        ],
      )),
    );
  }
}