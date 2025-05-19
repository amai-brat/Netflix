import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/personal_info/bloc/personal_info_bloc.dart';

class ChangePasswordDialog extends StatefulWidget {
  const ChangePasswordDialog({super.key});

  @override
  State<ChangePasswordDialog> createState() => _ChangePasswordDialogState();
}

class _ChangePasswordDialogState extends State<ChangePasswordDialog> {
  final _formKey = GlobalKey<FormState>();

  final _currentPasswordController = TextEditingController();
  final _newPasswordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  String? _confirmPasswordError;

  @override
  void dispose() {
    _currentPasswordController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  String? passwordValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (value.length < 8) {
      return 'Минимальная длина пароля - 8 символов';
    } else if (value.length > 30) {
      return 'Максимальная длина пароля - 30 символов';
    } else if (!RegExp(r'^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@;.,$!%*?&]).+$').hasMatch(value)) {
      return 'Пароль должен содержать хотя бы одну букву, цифру и спецсимвол';
    }
    return null;
  }

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<PersonalInfoBloc, PersonalInfoState>(
      builder: (context, state) {
        final isLoading = state is PersonalInfoLoaded && state.isPasswordUpdating;
        final error = state is PersonalInfoLoaded ? state.passwordError : null;
        final success = state is PersonalInfoLoaded ? state.passwordUpdateSuccess : null;

        return Dialog(
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
            side: BorderSide(color: Colors.grey.shade300, width: 1),
          ),
          insetPadding: const EdgeInsets.symmetric(horizontal: 24),
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(24),
            child: Form(
              key: _formKey,
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  const Text(
                    'Смена пароля',
                    style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                  ),
                  const SizedBox(height: 24),
                  TextFormField(
                    controller: _currentPasswordController,
                    decoration: const InputDecoration(
                      labelText: 'Текущий пароль',
                      border: OutlineInputBorder(),
                      contentPadding: EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                    obscureText: true,
                    enabled: !isLoading,
                  ),
                  const SizedBox(height: 16),
                  TextFormField(
                    controller: _newPasswordController,
                    decoration: const InputDecoration(
                      labelText: 'Новый пароль',
                      border: OutlineInputBorder(),
                      contentPadding: EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    ),
                    obscureText: true,
                    enabled: !isLoading,
                    validator: passwordValidator,
                  ),
                  const SizedBox(height: 16),
                  TextFormField(
                    controller: _confirmPasswordController,
                    decoration: InputDecoration(
                      labelText: 'Введите пароль повторно',
                      border: const OutlineInputBorder(),
                      contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                      errorText: _confirmPasswordError,
                    ),
                    obscureText: true,
                    enabled: !isLoading,
                  ),
                  if (success != null) ...[
                    const SizedBox(height: 12),
                    Text(
                      "Пароль успешно изменен",
                      style: const TextStyle(color: Colors.green),
                    ),
                  ],
                  if (error != null) ...[
                    const SizedBox(height: 12),
                    Text(
                      error,
                      style: const TextStyle(color: Colors.red),
                    ),
                  ],
                  const SizedBox(height: 10),
                  Row(
                    mainAxisAlignment: MainAxisAlignment.center,
                    children: [
                      TextButton(
                        onPressed: isLoading ? null : () => Navigator.pop(context),
                        child: const Text('Отмена'),
                      ),
                      const SizedBox(width: 12),
                      TextButton(
                        onPressed: isLoading
                            ? null
                            : () {
                          final formValid = _formKey.currentState!.validate();
                          final passwordsMatch = _newPasswordController.text ==
                              _confirmPasswordController.text;

                          setState(() {
                            _confirmPasswordError = passwordsMatch
                                ? null
                                : 'Пароли не совпадают';
                          });

                          if (formValid && passwordsMatch) {
                            context.read<PersonalInfoBloc>().add(
                              ChangePasswordEvent(
                                oldPassword: _currentPasswordController.text,
                                newPassword: _newPasswordController.text,
                              ),
                            );
                          }
                        },
                        child: const Text('Сохранить'),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}
