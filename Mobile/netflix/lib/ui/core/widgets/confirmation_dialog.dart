import 'package:flutter/material.dart';

class ConfirmationDialog extends StatelessWidget {
  final String title;
  final List<Widget> actions;

  const ConfirmationDialog({
    super.key,
    required this.title,
    required this.actions,
  });

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      shape: RoundedRectangleBorder(
        borderRadius: BorderRadius.all(Radius.circular(20)),
        side: BorderSide(color: Colors.grey[600]!),
      ),
      backgroundColor: Colors.black87,
      title: Text(title, textAlign: TextAlign.center),
      actions: actions,
      actionsAlignment: MainAxisAlignment.center,
    );
  }
}
