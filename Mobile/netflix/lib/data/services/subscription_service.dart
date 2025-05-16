import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/api_subscription.dart';
import 'package:netflix/data/models/api_user_subscription.dart';
import 'package:netflix/domain/dtos/bank_card_dto.dart';

import '../../utils/result.dart';

class SubscriptionService {
  final GraphQLClient _client;

  SubscriptionService(this._client);

  Result<T> _getErrorResult<T>(QueryResult<Object?> queryResult) {
    if (queryResult.exception?.linkException != null) {
      return Result.error(queryResult.exception!.linkException.toString());
    }
    return Result.error(
      queryResult.exception!.graphqlErrors.map((x) => x.message).join('\n'),
    );
  }

  Future<Result<List<ApiSubscription>>> getAllSubscriptions() async {
    try {
      const query = r'''
        query Subscriptions {
          subscriptions {
            __typename
            id
            name
            description
            maxResolution
            price
          }
        }
      ''';

      final options = QueryOptions(
        document: gql(query),
        fetchPolicy: FetchPolicy.networkOnly,
      );

      final result = await _client.query(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      return Result.ok(
        (result.data?['subscriptions'] as List)
            .map((s) => ApiSubscription.fromMap(s))
            .toList(),
      );
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<List<ApiUserSubscription>>> getUserSubscriptions() async {
    try {
      const query = r'''
        query UserSubscriptions {
          userSubscriptions {
            id
            userId
            subscriptionId
            expiresAt
            boughtAt
            transactionId
            status
          }
        }
      ''';

      final options = QueryOptions(
        document: gql(query),
        fetchPolicy: FetchPolicy.networkOnly,
      );

      final result = await _client.query(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      return Result.ok(
        (result.data?['userSubscriptions'] as List)
            .map((s) => ApiUserSubscription.fromMap(s))
            .toList(),
      );
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<ApiUserSubscription>> buySubscription({
    required int subscriptionId,
    required BankCardDto card,
  }) async {
    try {
      const mutation = r'''
        mutation BuySubscription($input: BuySubscriptionInput!) {
          buySubscription(input: $input) {
            userSubscription {
              id
              userId
              subscriptionId
              expiresAt
              boughtAt
              transactionId
              status
            }
          }
        }
      ''';

      final variables = {
        'input': {
          'subscriptionId': subscriptionId,
          'card': {
            'cardNumber': card.cardNumber,
            'cardOwner': card.cardOwner,
            'validThru': card.validThru,
            'cvc': card.cvc,
          },
        },
      };

      final options = MutationOptions(
        document: gql(mutation),
        variables: variables,
      );

      final result = await _client.mutate(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      return Result.ok(
        ApiUserSubscription.fromMap(
          result.data?['buySubscription']['userSubscription'],
        ),
      );
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<bool>> cancelSubscription({
    required int subscriptionId
  }) async {
    try {
      const mutation = r'''
        mutation CancelSubscription($input: CancelSubscriptionInput!) {
          cancelSubscription(input: $input) {
            success
          }
        }
      ''';

      final variables = {
        'input': {'subscriptionId': subscriptionId},
      };

      final options = MutationOptions(
        document: gql(mutation),
        variables: variables,
      );

      final result = await _client.mutate(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      return Result.ok(result.data?['cancelSubscription']['success']);
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
