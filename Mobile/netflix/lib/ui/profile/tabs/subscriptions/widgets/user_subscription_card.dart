import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/domain/models/user_subscription.dart';
import 'package:intl/intl.dart';
import 'package:netflix/ui/core/bloc/subscriptions/subscriptions_bloc.dart';
import 'package:netflix/ui/core/bloc/subscriptions/subscriptions_event.dart';
import 'package:netflix/ui/core/widgets/confirmation_dialog.dart';
import 'package:netflix/ui/profile/tabs/subscriptions/widgets/cancel_subscription_confirmation_dialog.dart';
import 'package:netflix/ui/subscriptions/widgets/check_mark.dart';

class UserSubscriptionCard extends StatelessWidget {
  final UserSubscription userSubscription;
  final Subscription subscription;

  const UserSubscriptionCard({
    super.key,
    required this.userSubscription,
    required this.subscription,
  });

  Widget _buildCheckedRow(BuildContext context, String text) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 8.0),
      child: Row(
        children: [
          Expanded(
            flex: 5,
            child: Text(
              text,
              style: Theme.of(context).textTheme.bodyLarge,
              textAlign: TextAlign.center,
            ),
          ),
          const Expanded(flex: 1, child: Center(child: CheckMark())),
        ],
      ),
    );
  }

  Widget _buildDetailRow(BuildContext context, String label, String value) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4.0),
      child: Row(
        children: [
          Expanded(
            flex: 2,
            child: Text(
              label,
              style: Theme.of(context).textTheme.bodyLarge,
              textAlign: TextAlign.left,
            ),
          ),
          Expanded(
            flex: 3,
            child: Text(
              value,
              style: Theme.of(context).textTheme.bodyLarge,
              textAlign: TextAlign.right,
            ),
          ),
        ],
      ),
    );
  }

  List<Widget> _buildUserSubscriptionDetails(BuildContext context) {
    return [
      _buildDetailRow(
        context,
        'Статус:',
        _translateStatus(userSubscription.status),
      ),
      _buildDetailRow(
        context,
        'Куплено:',
        _formatDate(userSubscription.boughtAt),
      ),
      _buildDetailRow(context, 'Цена:', '${subscription.price}'),
    ];
  }

  String _translateStatus(UserSubscriptionStatus status) {
    switch (status) {
      case UserSubscriptionStatus.pending:
        return 'Ожидание';
      case UserSubscriptionStatus.completed:
        return 'Активна';
      case UserSubscriptionStatus.failed:
        return 'Ошибка';
      case UserSubscriptionStatus.cancelled:
        return 'Отменена';
    }
  }

  String _formatDate(DateTime date) {
    return DateFormat('dd.MM.yyyy HH:mm').format(date);
  }

  Widget _buildCancelButton(BuildContext context) {
    return ElevatedButton(
      onPressed: () {
        final bloc = context.read<SubscriptionsBloc>();

        showDialog(
          context: context,
          builder: (context) {
            return BlocProvider.value(
              value: bloc,
              child: CancelSubscriptionConfirmationDialog(
                subscription: subscription,
              ),
            );
          },
        );
      },
      child: const Text('Отказаться'),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      decoration: BoxDecoration(
        color: Colors.white10,
        border: Border.all(color: Colors.grey, width: 1),
        borderRadius: const BorderRadius.all(Radius.circular(10)),
      ),
      padding: const EdgeInsets.symmetric(horizontal: 10, vertical: 10),
      child: Column(
        mainAxisSize: MainAxisSize.min,
        children: [
          ExpansionTile(
            shape: Border(),
            iconColor: Colors.white,
            title: Text(
              subscription.name,
              style: Theme.of(context).textTheme.headlineMedium!.copyWith(
                color: Theme.of(context).colorScheme.primary,
                fontWeight: FontWeight.bold,
              ),
            ),
            subtitle: Text(
              'Истекает: ${_formatDate(userSubscription.expiresAt)}',
              style: Theme.of(context).textTheme.bodyLarge,
            ),
            children: [
              SingleChildScrollView(
                child: Column(
                  children: [
                    _buildCheckedRow(context, subscription.description),
                    _buildCheckedRow(
                      context,
                      'Максимальное разрешение: ${subscription.maxResolution}',
                    ),
                    const Divider(),
                    ..._buildUserSubscriptionDetails(context),
                    const SizedBox(height: 10),
                    _buildCancelButton(context),
                  ],
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
