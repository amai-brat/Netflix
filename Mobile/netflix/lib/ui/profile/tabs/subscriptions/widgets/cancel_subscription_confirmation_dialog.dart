import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/ui/core/bloc/subscriptions/subscriptions_bloc.dart';
import 'package:netflix/ui/core/bloc/subscriptions/subscriptions_event.dart';
import 'package:netflix/ui/core/widgets/confirmation_dialog.dart';

class CancelSubscriptionConfirmationDialog extends StatelessWidget {
  final Subscription subscription;

  const CancelSubscriptionConfirmationDialog({
    super.key,
    required this.subscription,
  });

  @override
  Widget build(BuildContext context) {
    return ConfirmationDialog(
      title: 'Вы уверены?',
      actions: [
        ElevatedButton(
          onPressed: () {
            Navigator.of(context).pop();
          },
          child: const Text('Нет'),
        ),
        ElevatedButton(
          onPressed: () {
            context.read<SubscriptionsBloc>().add(
              CancelPressed(subscriptionId: subscription.id),
            );

            if (context.mounted) {
              Navigator.of(context).pop();
            }
          },
          child: const Text('Да'),
        ),
      ],
    );
  }
}
