import 'dart:math';

import 'package:collection/collection.dart';
import 'package:netflix/data/services/auth_service_mock.dart';
import 'package:netflix/domain/models/user.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/utils/result.dart';

class AuthRepositoryMock extends AuthRepository {
  final List<User> _users = [
    User(id: 1, email: 'a@a.a', role: 'user', name: 'ABOBA'),
  ];

  final AuthServiceMock _authService;

  bool? _isAuthenticated;

  AuthRepositoryMock({required AuthServiceMock authService})
    : _authService = authService;

  @override
  Future<Result<void>> signIn({
    required String email,
    required String password,
  }) async {
    final user = _users.firstWhereOrNull((element) => element.email == email);
    if (user == null) {
      return Result.error('Пользователь не найден');
    }

    await _authService.saveToken(user);
    return Result.ok(null);
  }

  @override
  Future<Result<void>> signUp({
    required String login,
    required String email,
    required String password,
  }) async {
    _users.add(
      User(
        id: Random().nextInt(10_000),
        role: 'user',
        email: email,
        name: login,
      ),
    );

    return Result.ok(null);
  }

  @override
  Future<Result<void>> signOut() async {
    final result = await _authService.removeToken();
    _isAuthenticated = null;
    return result;
  }

  @override
  Future<bool> get isAuthenticated async {
    // Status is cached
    if (_isAuthenticated != null) {
      return _isAuthenticated!;
    }
    // No status cached, fetch from storage
    try {
      await _fetch();
    } catch (e) {
      _isAuthenticated = null;
    }

    return _isAuthenticated ?? false;
  }

  Future<void> _fetch() async {
    final result = await _authService.fetchToken();
    switch (result) {
      case Ok<String?>():
        _isAuthenticated = result.value != null;
      case Error<String?>():
        break;
    }
  }

  @override
  Future<int?> get currentUserId async {
    final result = await _authService.fetchToken();
    switch (result) {
      case Ok<String?>():
        if (result.value == null) {
          return null;
        }
        return int.tryParse(result.value!);
      case Error<String?>():
        break;
    }

    return null;
  }
}
