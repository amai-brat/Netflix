part of 'personal_info_bloc.dart';

sealed class PersonalInfoEvent {}

class LoadUserInfoEvent extends PersonalInfoEvent {}

class ChangeEmailEvent extends PersonalInfoEvent {
  final String newEmail;

  ChangeEmailEvent(this.newEmail);
}

class ChangeBirthDateEvent extends PersonalInfoEvent {
  final String newBirthDate;

  ChangeBirthDateEvent(this.newBirthDate);
}

class ChangePasswordEvent extends PersonalInfoEvent {
  final String oldPassword;
  final String newPassword;

  ChangePasswordEvent({
    required this.oldPassword,
    required this.newPassword,
  });
}