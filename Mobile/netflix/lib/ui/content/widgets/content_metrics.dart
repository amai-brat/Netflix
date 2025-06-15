import 'package:flutter/material.dart';

import 'content_section.dart';

class ContentMetrics extends StatelessWidget {
  final int views;

  const ContentMetrics({super.key, required this.views});

  Widget _buildMetricRow(Widget widget, String metric) {
    return Row(children: [widget, Text(' : $metric')]);
  }

  @override
  Widget build(BuildContext context) {
    return ContentSection(
      title: 'Метрики',
      children: [
        _buildMetricRow(const Icon(Icons.remove_red_eye), views.toString())
      ],
    );
  }
}