import 'package:flutter/material.dart';
import 'package:netflix/app.dart';
import 'package:netflix/utils/di.dart' as di;

void main() {
  di.setupLocator();
  runApp(const NetflixApp());
}