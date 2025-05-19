import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:netflix/data/models/api_sign_in_dto.dart';
import 'package:netflix/data/models/api_sign_out_dto.dart';
import 'package:netflix/data/models/api_sign_up_dto.dart';
import 'package:netflix/data/services/auth_service.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/utils/jwt_decoder.dart';
import 'package:netflix/utils/result.dart';

import '../../utils/consts.dart';

class AuthRepositoryImpl extends AuthRepository {
  final AuthService _authService;
  final FlutterSecureStorage _secureStorage;

  AuthRepositoryImpl({
    required AuthService authService,
    required FlutterSecureStorage secureStorage,
  }) : _authService = authService,
       _secureStorage = secureStorage;

  @override
  Future<Result<void>> signIn({
    required String email,
    required String password,
  }) async {
    final result = await _authService.signIn(email: email, password: password);
    switch (result) {
      case Ok<ApiSignInDto>():
        final dto = result.value;

        if (dto.tokens == null) {
          return Result.error(dto.message ?? 'Sign in failed');
        }

        await _secureStorage.write(
          key: Consts.accessToken,
          value: dto.tokens!.accessToken,
        );

        return Result.ok(null);
      case Error<ApiSignInDto>():
        return Result.error(result.error);
    }
  }

  @override
  Future<Result<void>> signUp({
    required String login,
    required String email,
    required String password,
  }) async {
    final result = await _authService.signUp(
      login: login,
      email: email,
      password: password,
    );
    switch (result) {
      case Ok<ApiSignUpDto>():
        return Result.ok(null);
      case Error<ApiSignUpDto>():
        return Result.error(result.error);
    }
  }

  @override
  Future<Result<void>> signOut() async {
    final result = await _authService.signOut();

    await _secureStorage.delete(key: Consts.accessToken);

    switch (result) {
      case Ok<ApiSignOutDto>():
        return Result.ok(null);
      case Error<ApiSignOutDto>():
        return Result.error(result.error);
    }
  }

  @override
  Future<bool> get isAuthenticated async {
    final token = await _secureStorage.read(
      key: Consts.accessToken,
    );

    return token != null;
  }

  @override
  Future<int?> get currentUserId async {
    final token = await _secureStorage.read(
      key: Consts.accessToken,
    );

    if (token == null) {
      return null;
    }
    return int.tryParse(JwtDecoder.decode(token)['id']);
  }
}
