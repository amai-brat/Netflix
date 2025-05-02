import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_bloc.dart';
import 'package:netflix/ui/subscriptions/widgets/bank_card_form.dart';
import 'package:netflix/ui/subscriptions/widgets/check_mark.dart';

class SubscriptionCard extends StatelessWidget {
  final Subscription subscription;
  final bool isPurchasable;

  const SubscriptionCard({
    super.key,
    required this.subscription,
    required this.isPurchasable,
  });

  Widget _buildCheckedRow(BuildContext context, String text) {
    return Expanded(
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
          const Expanded(
            flex: 1,
            child: Center(child: CheckMark()),
          ),
        ],
      ),
    );
  }

  Widget _buildPurchaseButton(BuildContext context, bool isPurchasable) {
    return isPurchasable
        ? ElevatedButton(
          onPressed: () {
            final bloc = context.read<SubscriptionsBloc>();

            showDialog(
              context: context,
              builder: (context) {
                return BlocProvider<SubscriptionsBloc>.value(
                  value: bloc,
                  child: BankCardForm(subscription: subscription),
                );
              },
            );
          },
          child: const Text('Купить'),
        )
        : ElevatedButton(
          onPressed: null,
          child: const Text('Подписка куплена'),
        );
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      constraints: BoxConstraints(minHeight: 200, maxHeight: 500),
      decoration: BoxDecoration(
        color: Colors.white10,
        border: Border.all(color: Colors.grey, width: 1),
        borderRadius: BorderRadius.all(Radius.circular(10)),
      ),
      padding: EdgeInsets.symmetric(horizontal: 10, vertical: 10),
      child: IntrinsicHeight(
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            Text(
              subscription.name,
              style: Theme.of(context).textTheme.headlineMedium!.copyWith(
                color: ColorScheme.of(context).primary,
                fontWeight: FontWeight.bold,
              ),
            ),
            Text(
              'Цена: ${subscription.price}',
              style: Theme.of(context).textTheme.bodyLarge,
            ),
            const Divider(),
            Expanded(
              child: Column(
                children: [
                  _buildCheckedRow(context, subscription.description),
                  _buildCheckedRow(
                    context,
                    'Максимальное разрешение: ${subscription.maxResolution}',
                  ),
                ],
              ),
            ),
            SizedBox(height: 20),
            _buildPurchaseButton(context, isPurchasable),
          ],
        ),
      ),
    );
  }
}
