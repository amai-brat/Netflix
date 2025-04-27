import 'dart:io';

import 'package:netflix/utils/result.dart';
import 'package:path_provider/path_provider.dart';

class AuthServiceMock {
  static const String authFile = 'auth.json';

  Future<String> getDirectory() async {
    final dir = await getApplicationDocumentsDirectory();
    return dir.path;
  }

  Future<Result<String?>> fetchToken() async {
    final dir = await getDirectory();
    final token = await File('$dir/$authFile').readAsString();

    return Result.ok(token);
  }

  Future<Result<void>> saveToken() async {
    final dir = await getDirectory();
    await File('$dir/$authFile').writeAsString('token');

    return Result.ok(null);
  }

  Future<Result<void>> removeToken() async {
    final dir = await getDirectory();
    await File('$dir/$authFile').delete();

    return Result.ok(null);
  }
}
