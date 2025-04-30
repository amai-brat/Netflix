import 'package:flutter/material.dart';

class CheckMark extends StatelessWidget {
  final double width;
  final double height;

  const CheckMark({super.key, this.width = 20, this.height = 20});

  @override
  Widget build(BuildContext context) => Image(
    image: AssetImage('assets/images/check_mark.png'),
    width: width,
    height: height,
  );
}
