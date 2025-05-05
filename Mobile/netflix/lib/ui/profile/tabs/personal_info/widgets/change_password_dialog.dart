import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/personal_info/bloc/personal_info_bloc.dart';

class ChangePasswordDialog extends StatefulWidget {
  const ChangePasswordDialog({super.key});

  @override
  State<ChangePasswordDialog> createState() => _ChangePasswordDialogState();
}

class _ChangePasswordDialogState extends State<ChangePasswordDialog> {
  final _currentPasswordController = TextEditingController();
  final _newPasswordController = TextEditingController();
  final _confirmPasswordController = TextEditingController();

  bool _submitted = false;

  @override
  void dispose() {
    _currentPasswordController.dispose();
    _newPasswordController.dispose();
    _confirmPasswordController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<PersonalInfoBloc, PersonalInfoState>(
      builder: (context, state) {
        final isLoading = state is PersonalInfoLoaded && state.isPasswordUpdating;
        final error = state is PersonalInfoLoaded ? state.passwordError : null;
        final success = state is PersonalInfoLoaded ? state.passwordUpdateSuccess : null;

        String? newPasswordError;
        String? confirmPasswordError;

        if (_submitted) {
          if (_newPasswordController.text.length < 8) {
            newPasswordError = 'Пароль должен быть не менее 8 символов';
          }
          if (_newPasswordController.text != _confirmPasswordController.text) {
            confirmPasswordError = 'Пароли не совпадают';
          }
        }

        return Dialog(
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(12),
            side: BorderSide(color: Colors.grey.shade300, width: 1),
          ),
          insetPadding: const EdgeInsets.symmetric(horizontal: 24),
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(24),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                const Text(
                  'Смена пароля',
                  style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                ),
                const SizedBox(height: 24),
                TextField(
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
                TextField(
                  controller: _newPasswordController,
                  decoration: InputDecoration(
                    labelText: 'Новый пароль',
                    border: const OutlineInputBorder(),
                    contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    errorText: newPasswordError,
                  ),
                  obscureText: true,
                  enabled: !isLoading,
                ),
                const SizedBox(height: 16),
                TextField(
                  controller: _confirmPasswordController,
                  decoration: InputDecoration(
                    labelText: 'Подтвердите пароль',
                    border: const OutlineInputBorder(),
                    contentPadding: const EdgeInsets.symmetric(horizontal: 12, vertical: 10),
                    errorText: confirmPasswordError ?? error,
                  ),
                  obscureText: true,
                  enabled: !isLoading,
                ),
                if (success != null) ...[
                  const SizedBox(height: 12),
                  Text(
                    success,
                    style: const TextStyle(color: Colors.green),
                  ),
                ],
                const SizedBox(height: 20),
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
                        setState(() {
                          _submitted = true;
                        });

                        if (_newPasswordController.text.length >= 8 &&
                            _newPasswordController.text == _confirmPasswordController.text) {
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
        );
      },
    );
  }
}