import 'package:flutter/material.dart';
import 'package:flutter_dotenv/flutter_dotenv.dart';
import 'package:netflix/app.dart';
import 'package:netflix/utils/di.dart' as di;

Future<void> main() async {
  await dotenv.load(fileName: ".env.template");

  di.setupLocator();
  runApp(const NetflixApp());
}