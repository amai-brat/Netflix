import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/user_info.dart';
import 'package:netflix/domain/use_cases/get_user_info_use_case.dart';
import 'package:netflix/domain/use_cases/change_email_use_case.dart';
import 'package:netflix/domain/use_cases/change_birthdate_use_case.dart';
import 'package:netflix/domain/use_cases/change_password_use_case.dart';
import 'package:netflix/utils/di.dart';
import 'package:netflix/utils/result.dart';

part 'personal_info_event.dart';
part 'personal_info_state.dart';

class PersonalInfoBloc extends Bloc<PersonalInfoEvent, PersonalInfoState> {
  final GetUserInfoUseCase getUserInfo;
  final ChangeEmailUseCase changeEmail;
  final ChangeBirthDateUseCase changeBirthDate;
  final ChangePasswordUseCase changePassword;

  PersonalInfoBloc({
    required this.getUserInfo,
    required this.changeEmail,
    required this.changeBirthDate,
    required this.changePassword,
  }) : super(PersonalInfoLoading()) {
    on<LoadUserInfoEvent>(_onLoadUserInfo);
    on<ChangeEmailEvent>(_onChangeEmail);
    on<ChangeBirthDateEvent>(_onChangeBirthDate);
    on<ChangePasswordEvent>(_onChangePassword);
  }

  static PersonalInfoBloc createViaLocator() {
    return PersonalInfoBloc(
      getUserInfo: locator<GetUserInfoUseCase>(),
      changeEmail: locator<ChangeEmailUseCase>(),
      changeBirthDate: locator<ChangeBirthDateUseCase>(),
      changePassword: locator<ChangePasswordUseCase>(),
    );
  }

  Future<void> _onLoadUserInfo(
      LoadUserInfoEvent event,
      Emitter<PersonalInfoState> emit,
      ) async {
    emit(PersonalInfoLoading());
    final result = await getUserInfo.execute();

    switch (result) {
      case Ok(value: final userInfo):
        emit(PersonalInfoLoaded(userInfo: userInfo));
      case Error(error: final error):
        emit(PersonalInfoError(message: error));
    }
  }

  Future<void> _onChangeEmail(
      ChangeEmailEvent event,
      Emitter<PersonalInfoState> emit,
      ) async {
    if (state is! PersonalInfoLoaded) return;

    emit((state as PersonalInfoLoaded).copyWith(isEmailUpdating: true));

    final result = await changeEmail.execute(event.newEmail);

    switch (result) {
      case Ok(value: final updatedUser):
        emit(PersonalInfoLoaded(
          userInfo: updatedUser,
          emailUpdateSuccess: true,
        ));
      case Error(error: final error):
        emit((state as PersonalInfoLoaded).copyWith(
          isEmailUpdating: false,
          emailError: error,
        ));
    }
  }

  Future<void> _onChangeBirthDate(
      ChangeBirthDateEvent event,
      Emitter<PersonalInfoState> emit,
      ) async {
    if (state is! PersonalInfoLoaded) return;

    emit((state as PersonalInfoLoaded).copyWith(isBirthDateUpdating: true));

    final result = await changeBirthDate.execute(event.newBirthDate);

    switch (result) {
      case Ok(value: final updatedUser):
        emit(PersonalInfoLoaded(
          userInfo: updatedUser,
          birthDateUpdateSuccess: true,
        ));
      case Error(error: final error):
        emit((state as PersonalInfoLoaded).copyWith(
          isBirthDateUpdating: false,
          birthDateError: error,
        ));
    }
  }

  Future<void> _onChangePassword(
      ChangePasswordEvent event,
      Emitter<PersonalInfoState> emit,
      ) async {
    if (state is! PersonalInfoLoaded) return;

    emit((state as PersonalInfoLoaded).copyWith(isPasswordUpdating: true));

    final result = await changePassword.execute(
      oldPassword: event.oldPassword,
      newPassword: event.newPassword,
    );

    switch (result) {
      case Ok(value: final successMessage):
        emit((state as PersonalInfoLoaded).copyWith(
          isPasswordUpdating: false,
          passwordUpdateSuccess: successMessage,
        ));
      case Error(error: final error):
        emit((state as PersonalInfoLoaded).copyWith(
          isPasswordUpdating: false,
          passwordError: error,
        ));
    }
  }
}