import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_bloc.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_event.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_state.dart';
import 'package:netflix/ui/subscriptions/widgets/subscription_card.dart';

class SubscriptionsPage extends StatelessWidget {
  const SubscriptionsPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) =>
              SubscriptionsBloc.createViaLocator()
                ..add(SubscriptionsPageOpened()),
      child: Scaffold(
        appBar: AppBar(title: const Text('Подписки')),
        body: _buildBody(),
      ),
    );
  }

  Widget _buildBody() {
    return BlocConsumer<SubscriptionsBloc, SubscriptionsState>(
      listener: (context, state) {
        if (state.error.isNotEmpty) {
          ScaffoldMessenger.of(
            context,
          ).showSnackBar(SnackBar(content: Text(state.error)));
        }
      },
      builder: (context, state) {
        if (state.status == SubscriptionsStatus.loading) {
          return Center(child: CircularProgressIndicator());
        }

        return Container(
          padding: EdgeInsets.symmetric(horizontal: 20),
          child: Center(
            child: SingleChildScrollView(
              child: Column(
                spacing: 10,
                children:
                    state.subscriptions
                        .map(
                          (s) => SubscriptionCard(
                            isPurchasable:
                                !context.read<SubscriptionsBloc>().isPurchased(
                                  s,
                                ),
                            subscription: s,
                          ),
                        )
                        .toList(),
              ),
            ),
          ),
        );
      },
    );
  }
}
