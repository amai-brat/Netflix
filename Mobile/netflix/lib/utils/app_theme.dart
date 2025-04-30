import 'package:flutter/material.dart';

class AppTheme {
  AppTheme._();

  static final _theme = ThemeData(
    colorScheme: ColorScheme.dark(
      primary: Color(0xFFE50914),
      secondary: Color(0xFFE50914),
      surface: Color(0xFF000000),
    ),
    elevatedButtonTheme: ElevatedButtonThemeData(
      style: ElevatedButton.styleFrom(
        backgroundColor: Color(0xFFE50914),
        foregroundColor: Color(0xFFFFFFFF),
        disabledBackgroundColor: Color(0xFF0BB000),
        disabledForegroundColor: Color(0xFFFFFFFF)
      ),
    ),
    inputDecorationTheme: InputDecorationTheme(
      fillColor: Colors.grey[900],
      filled: true,
      hintStyle: TextStyle(color: Colors.grey[400]),
      labelStyle: TextStyle(color: Colors.white),
      border: OutlineInputBorder(
        borderRadius: BorderRadius.all(Radius.circular(25)),
        borderSide: BorderSide.none,
      ),
    ),
  );

  static get theme => _theme;
}