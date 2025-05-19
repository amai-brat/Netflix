import 'package:graphql_flutter/graphql_flutter.dart';
import 'package:intl/intl.dart';
import 'package:netflix/data/models/user_info_dto.dart';
import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/utils/result.dart';

class PersonalInfoService {
  final GraphQLClient _client;

  PersonalInfoService(this._client);

  Result<T> _getErrorResult<T>(QueryResult<Object?> queryResult) {
    if (queryResult.exception?.linkException != null) {
      return Result.error(queryResult.exception!.linkException.toString());
    }
    return Result.error(
      queryResult.exception!.graphqlErrors.map((x) => x.message).join('\n'),
    );
  }

  Future<Result<UserInfo>> getUserInfo() async {
    try {
      const query = r'''
        query PersonalInfo {
          personalInfo {
            nickname
            birthDay
            email
            profilePictureUrl
          }
        }
      ''';

      final options = QueryOptions(
        document: gql(query),
        fetchPolicy: FetchPolicy.networkOnly,
      );

      final result = await _client.query(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      final data = result.data?['personalInfo'];
      if (data == null) {
        return Result.error('Personal info not found');
      }

      final dto = UserInfoDto.fromMap(data as Map<String, dynamic>);
      return Result.ok(dto.toUserInfo());
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<UserInfo>> changeEmail(String newEmail) async {
    try {
      const mutation = r'''
        mutation ChangeEmail($input: ChangeEmailInput!) {
          changeEmail(input: $input) {
            personalInfoDto {
              nickname
              birthDay
              email
              profilePictureUrl
            }
          }
        }
      ''';

      final options = MutationOptions(
        document: gql(mutation),
        variables: {
          'input': {
            'newEmail': newEmail,
          },
        },
      );

      final result = await _client.mutate(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      final data = result.data?['changeEmail']?['personalInfoDto'];
      if (data == null) {
        return Result.error('No user info returned after email change');
      }

      final dto = UserInfoDto.fromMap(data as Map<String, dynamic>);
      return Result.ok(dto.toUserInfo());
    } catch (e) {
      return Result.error(e.toString());
    }
  }


  Future<Result<UserInfo>> changeBirthDate(String newBirthDate) async {
    try {
      const mutation = r'''
        mutation ChangeBirthDay($input: ChangeBirthDayInput!) {
          changeBirthDay(input: $input) {
            personalInfoDto {
              nickname
              birthDay
              email
              profilePictureUrl
            }
          }
        }
      ''';

      final parsed = DateFormat('dd.MM.yyyy').parseStrict(newBirthDate);
      final iso = DateFormat('yyyy-MM-dd').format(parsed);

      final options = MutationOptions(
        document: gql(mutation),
        variables: {
          'input': {
            'birthDay': iso,
          },
        },
      );

      final result = await _client.mutate(options);
      if (result.hasException) {
        return _getErrorResult(result);
      }

      final data = result.data?['changeBirthDay']?['personalInfoDto'];
      if (data == null) {
        return Result.error('No user info returned after birth date change');
      }

      final dto = UserInfoDto.fromMap(data as Map<String, dynamic>);
      return Result.ok(dto.toUserInfo());
    } catch (e) {
      return Result.error(e.toString());
    }
  }

  Future<Result<String>> changePassword({
    required String oldPassword,
    required String newPassword,
  }) async {
    try {
      const mutation = r'''
        mutation ChangePassword($input: ChangePasswordInput!) {
          changePassword(input: $input) {
            changePasswordDto {
              email
            }
          }
        }
      ''';

      final options = MutationOptions(
        document: gql(mutation),
        variables: {
          'input': {
            'passwords': {
              'previousPassword': oldPassword,
              'newPassword': newPassword,
            },
          },
        },
      );

      final result = await _client.mutate(options);

      if (result.hasException) {
        return _getErrorResult(result);
      }

      final email = result.data?['changePassword']?['changePasswordDto']?['email'];
      if (email == null) {
        return Result.error('Email not returned after password change');
      }

      return Result.ok(email as String);
    } catch (e) {
      return Result.error(e.toString());
    }
  }
}