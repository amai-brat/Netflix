import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:netflix/data/models/api_sign_in_dto.dart';
import 'package:netflix/data/models/api_sign_out_dto.dart';
import 'package:netflix/data/models/api_sign_up_dto.dart';

import '../../utils/result.dart';

class AuthService {
  final GraphQLClient _client;

  AuthService(this._client);

  Result<T> _getErrorResult<T>(QueryResult<Object?> queryResult) {
    if (queryResult.exception?.linkException != null) {
      return Result.error(queryResult.exception!.linkException.toString());
    }
    return Result.error(
      queryResult.exception!.graphqlErrors.map((x) => x.message).join('\n'),
    );
  }

  Future<Result<ApiSignUpDto>> signUp({
    required String login,
    required String email,
    required String password,
  }) async {
    try {
      const mutation = r'''
        mutation SignUp($input: SignUpInput!) {
          signUp(input: $input) {
            id
          }
        }
      ''';

      final variables = {
        'input': {'login': login, 'email': email, 'password': password},
      };

      final options = MutationOptions(
        document: gql(mutation),
        variables: variables,
      );

      final result = await _client.mutate(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      return Result.ok(ApiSignUpDto.fromMap(result.data?['signUp']));
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<ApiSignInDto>> signIn({
    required String email,
    required String password,
  }) async {
    try {
      const mutation = r'''
        mutation SignIn($input: SignInInput!) {
          signIn(input: $input) {
            code
            message
            tokens {
              accessToken
              refreshToken
            }
          }
        }
      ''';

      final variables = {
        'input': {'email': email, 'password': password, 'rememberMe': true},
      };

      final options = MutationOptions(
        document: gql(mutation),
        variables: variables,
      );

      final result = await _client.mutate(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      return Result.ok(ApiSignInDto.fromMap(result.data?['signIn']));
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<ApiSignOutDto>> signOut() async {
    try {
      const mutation = r'''
        mutation SignOut {
          signOut {
            code
            message
          }
        }
      ''';

      final options = MutationOptions(document: gql(mutation));

      final result = await _client.mutate(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      return Result.ok(ApiSignOutDto.fromMap(result.data?['signOut']));
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}
