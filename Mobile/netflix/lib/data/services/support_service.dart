import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/support_chat_history_message.dart';

class SupportService {
  final GraphQLClient _client;

  SupportService(this._client);

  Future<List<SupportChatHistoryMessage>> getHistory() async {
    const query = r'''
      query GetUserSupportChatHistory {
        userSupportChatHistory {
          files {
            name
            src
            type
          }
          role
          text
        }
      }
    ''';

    final options = QueryOptions(
        document: gql(query),
        fetchPolicy: FetchPolicy.networkOnly
    );

    final result = await _client.query(options);

    if (result.hasException) {
      final error = result.exception!.linkException ?? result.exception!.graphqlErrors.map((e) => e.message).join('\n');
      throw Exception(error);
    }

    final history = result.data?['userSupportChatHistory'] ?? [];
    return (history as List).map((json) => SupportChatHistoryMessage.fromMap(json)).toList();
  }
}