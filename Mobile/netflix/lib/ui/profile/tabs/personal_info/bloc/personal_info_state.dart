part of 'personal_info_bloc.dart';

sealed class PersonalInfoState {}

class PersonalInfoLoading extends PersonalInfoState {}

class PersonalInfoLoaded extends PersonalInfoState {
  final UserInfo userInfo;
  final bool isEmailUpdating;
  final bool isBirthDateUpdating;
  final bool isPasswordUpdating;
  final String? emailError;
  final String? birthDateError;
  final String? passwordError;
  final bool? emailUpdateSuccess;
  final bool? birthDateUpdateSuccess;
  final String? passwordUpdateSuccess;

  PersonalInfoLoaded({
    required this.userInfo,
    this.isEmailUpdating = false,
    this.isBirthDateUpdating = false,
    this.isPasswordUpdating = false,
    this.emailError,
    this.birthDateError,
    this.passwordError,
    this.emailUpdateSuccess,
    this.birthDateUpdateSuccess,
    this.passwordUpdateSuccess,
  });

  PersonalInfoLoaded copyWith({
    UserInfo? userInfo,
    bool? isEmailUpdating,
    bool? isBirthDateUpdating,
    bool? isPasswordUpdating,
    String? emailError,
    String? birthDateError,
    String? passwordError,
    bool? emailUpdateSuccess,
    bool? birthDateUpdateSuccess,
    String? passwordUpdateSuccess,
  }) {
    return PersonalInfoLoaded(
      userInfo: userInfo ?? this.userInfo,
      isEmailUpdating: isEmailUpdating ?? this.isEmailUpdating,
      isBirthDateUpdating: isBirthDateUpdating ?? this.isBirthDateUpdating,
      isPasswordUpdating: isPasswordUpdating ?? this.isPasswordUpdating,
      emailError: emailError ?? this.emailError,
      birthDateError: birthDateError ?? this.birthDateError,
      passwordError: passwordError ?? this.passwordError,
      emailUpdateSuccess: emailUpdateSuccess ?? this.emailUpdateSuccess,
      birthDateUpdateSuccess: birthDateUpdateSuccess ?? this.birthDateUpdateSuccess,
      passwordUpdateSuccess: passwordUpdateSuccess ?? this.passwordUpdateSuccess,
    );
  }
}

class PersonalInfoError extends PersonalInfoState {
  final String message;

  PersonalInfoError({required this.message});
}