import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:netflix/ui/core/bloc/subscriptions/subscriptions_bloc.dart';
import 'package:netflix/ui/core/bloc/subscriptions/subscriptions_event.dart';
import 'package:netflix/ui/core/bloc/subscriptions/subscriptions_state.dart';
import 'package:netflix/ui/profile/tabs/subscriptions/widgets/user_subscription_card.dart';
import 'package:netflix/utils/routes.dart';

class SubscriptionsView extends StatelessWidget {
  const SubscriptionsView({super.key});

  Widget _buildSubscriptionsNavigationButton(BuildContext context) {
    return Center(
      child: ElevatedButton(
        onPressed: () async {
          await context.push(Routes.subscriptions);

          if (context.mounted) {
            context.read<SubscriptionsBloc>().add(SubscriptionsPageOpened());
          }
        },
        child: Text('Купить подписку'),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) =>
              SubscriptionsBloc.createViaLocator()
                ..add(SubscriptionsPageOpened()),
      child: Scaffold(
        appBar: AppBar(title: const Text('Мои подписки')),
        body: BlocBuilder<SubscriptionsBloc, SubscriptionsState>(
          builder: (context, state) {
            if (state.status == SubscriptionsStatus.loading) {
              return Center(child: CircularProgressIndicator());
            }

            return Container(
              padding: EdgeInsets.symmetric(horizontal: 20),
              child: Center(
                child: ListView.separated(
                  shrinkWrap: true,
                  itemBuilder: (context, index) {
                    if (index == state.userSubscriptions.length) {
                      return _buildSubscriptionsNavigationButton(context);
                    }

                    final us = state.userSubscriptions[index];
                    final sub = state.subscriptions.firstWhere(
                      (sub) => sub.id == us.subscriptionId,
                    );

                    return UserSubscriptionCard(
                      userSubscription: us,
                      subscription: sub,
                    );
                  },
                  separatorBuilder: (context, index) {
                    if (index == state.userSubscriptions.length - 1) {
                      return const SizedBox(height: 20);
                    }
                    return const SizedBox(height: 10);
                  },
                  itemCount: state.userSubscriptions.length + 1,
                ),
              ),
            );
          },
        ),
      ),
    );
  }
}
