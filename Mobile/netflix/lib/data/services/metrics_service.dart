import 'dart:async';

import 'package:http/http.dart' as http;
import 'package:dart_amqp/dart_amqp.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:netflix/data/models/content_view_count.dart';

class MetricsService {
  final Map<String, Queue> _contentViewsQueues = {};

  late Client _client;
  bool _isConnected = false;

  Future<void> connect() async {
    if (_isConnected) return;

    _client = Client(
      settings: ConnectionSettings(
        host: dotenv.env["RABBIT_HOST"]!,
        port: int.parse(dotenv.env["RABBIT_PORT"] ?? "5672"),
        authProvider: PlainAuthenticator(
          dotenv.env["RABBIT_USERNAME"]!,
          dotenv.env["RABBIT_PASSWORD"]!,
        ),
      ),
    );

    await _client.connect();
    _isConnected = true;
  }

  Future<void> disconnect({required String streamCancellationToken}) async {
    _contentViewsQueues[streamCancellationToken]?.delete();
    _contentViewsQueues.remove(streamCancellationToken);

    if (_contentViewsQueues.isEmpty) {
      await _client.close();
      _isConnected = false;
    }
  }

  Future<({Stream<int> stream, String streamCancellationToken})>
  getContentViews(int contentId) async {
    await connect();

    final channel = await _client.channel();
    final exchange = await channel.exchange(
      'content-page-views',
      ExchangeType.DIRECT,
    );
    final queue = await channel.queue('', durable: false, autoDelete: true);
    await queue.bind(exchange, '$contentId');

    final controller = StreamController<int>();

    void onListen() async {
      try {
        final consumer = await queue.consume();
        consumer.listen(
          (message) {
            controller.add(
              ContentViewCount.fromMap(
                message.payloadAsJson as Map<String, dynamic>,
              ).views,
            );
          },
          onError: (error) {
            controller.addError(error);
          },
        );
      } catch (error) {
        controller.addError(error);
      }
    }

    controller.onListen = onListen;

    _contentViewsQueues[queue.name] = queue;

    return (stream: controller.stream, streamCancellationToken: queue.name);
  }

  Future<void> sendContentViewed({required int contentId}) async {
    await http.post(
      Uri.http(dotenv.env["METRICS_BASE_URL"]!, "metrics/content/$contentId"),
    );
  }
}
