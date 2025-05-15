import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_event.dart';
import 'package:netflix/ui/auth/bloc/auth_state.dart';
import 'package:netflix/ui/core/bloc/user/user_bloc.dart';
import 'package:netflix/ui/core/bloc/user/user_event.dart';
import 'package:netflix/ui/core/bloc/user/user_state.dart';
import 'package:provider/provider.dart';

class AuthForm extends StatefulWidget {
  const AuthForm({super.key});

  String? loginValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (value.length < 4) {
      return 'Минимальная длина логина - 4 символов';
    } else if (value.length > 25) {
      return 'Максимальная длина логина - 25 символов';
    } else if (!RegExp(r'^[a-zA-Z0-9_]+$').hasMatch(value)) {
      return 'Запрещённые символы';
    }
    return null;
  }

  String? emailValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (!RegExp(
      r'^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]+$',
    ).hasMatch(value)) {
      return 'Неправильный адрес почты';
    }
    return null;
  }

  String? passwordValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (value.length < 8) {
      return 'Минимальная длина пароля - 8 символов';
    } else if (value.length > 30) {
      return 'Максимальная длина пароля - 30 символов';
    } else if (!RegExp(
      r'^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@;.,$!%*?&]).+$',
    ).hasMatch(value)) {
      return 'Пароль должен содержать хотя бы одну букву, цифру и спецсимвол';
    }
    return null;
  }

  @override
  State<AuthForm> createState() => _AuthFormState();
}

class _AuthFormState extends State<AuthForm> {
  final _formKey = GlobalKey<FormState>();
  final _emailFieldKey = GlobalKey<FormFieldState>();
  final _loginFieldKey = GlobalKey<FormFieldState>();
  final _passwordFieldKey = GlobalKey<FormFieldState>();

  @override
  Widget build(BuildContext context) {
    return BlocListener<UserBloc, UserState>(
      listener: (context, state) {
        if (state.status == UserStatus.signUpSuccess) {
          context.read<AuthBloc>().add(
            AuthFormTypeChanged(AuthFormType.signin),
          );
        }
        if (state.error.isNotEmpty) {
          ScaffoldMessenger.of(
            context,
          ).showSnackBar(SnackBar(content: Text(state.error)));
        }
      },
      child: BlocBuilder<AuthBloc, AuthState>(
        builder: (context, state) {
          return Form(
            key: _formKey,
            child: Column(
              children: [
                if (state.formType == AuthFormType.signup) ...[
                  TextFormField(
                    key: _loginFieldKey,
                    decoration: InputDecoration(hintText: 'Логин'),
                    onChanged: (value) {
                      context.read<AuthBloc>().add(
                        AuthFieldsChanged(login: value),
                      );
                    },
                    validator: widget.loginValidator,
                  ),
                  SizedBox(height: 8),
                ],
                TextFormField(
                  key: _emailFieldKey,
                  decoration: InputDecoration(hintText: 'Почта'),
                  onChanged: (value) {
                    context.read<AuthBloc>().add(
                      AuthFieldsChanged(email: value),
                    );
                  },
                  validator: widget.emailValidator,
                ),
                SizedBox(height: 8),
                TextFormField(
                  key: _passwordFieldKey,
                  decoration: InputDecoration(hintText: 'Пароль'),
                  obscureText: true,
                  onChanged:
                      (value) => context.read<AuthBloc>().add(
                        AuthFieldsChanged(password: value),
                      ),
                  validator: (value) {
                    if (state.formType == AuthFormType.signin) return null;
                    return widget.passwordValidator(value);
                  },
                ),
                SizedBox(height: 8),
                ElevatedButton(
                  style: ElevatedButtonTheme.of(context).style,
                  onPressed: () {
                    if (_formKey.currentState!.validate()) {
                      switch (state.formType) {
                        case AuthFormType.signup:
                          context.read<UserBloc>().add(
                            SignUpPressed(
                              login: state.login,
                              email: state.email,
                              password: state.password,
                            ),
                          );
                        case AuthFormType.signin:
                          context.read<UserBloc>().add(
                            SignInPressed(
                              email: state.email,
                              password: state.password,
                            ),
                          );
                      }
                    }
                  },
                  child: Text(switch (state.formType) {
                    AuthFormType.signup => 'Зарегистрироваться',
                    AuthFormType.signin => 'Войти',
                  }),
                ),
                SizedBox(height: 10),
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    Text(switch (state.formType) {
                      AuthFormType.signup => 'Уже есть аккаунт?',
                      AuthFormType.signin => 'Новенький в Netflix?',
                    }, style: Theme.of(context).textTheme.labelSmall),
                    TextButton(
                      onPressed:
                          () => context.read<AuthBloc>().add(
                            AuthFormTypeChanged(switch (state.formType) {
                              AuthFormType.signup => AuthFormType.signin,
                              AuthFormType.signin => AuthFormType.signup,
                            }),
                          ),
                      child: Text(switch (state.formType) {
                        AuthFormType.signup => 'Войти',
                        AuthFormType.signin => 'Зарегистрироваться',
                      }, style: Theme.of(context).textTheme.bodyMedium),
                    ),
                  ],
                ),
              ],
            ),
          );
        },
      ),
    );
  }
}
