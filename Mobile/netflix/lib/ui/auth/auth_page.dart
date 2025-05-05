import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_state.dart';
import 'package:netflix/ui/auth/widgets/auth_form.dart';

class AuthPage extends StatelessWidget {
  const AuthPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create: (context) => AuthBloc(),
      child: Scaffold(
        appBar: AppBar(title: const Text('Авторизация')),
        body: BlocBuilder<AuthBloc, AuthState>(
          builder: (context, state) {
            return Center(
              child: Container(
                // margin: EdgeInsets.symmetric(vertical: 5, horizontal: 5),
                padding: EdgeInsets.symmetric(horizontal: 20, vertical: 20),
                decoration: BoxDecoration(
                  borderRadius: BorderRadius.all(Radius.circular(25)),
                  color: Theme.of(context).primaryColor,
                ),
                child: SingleChildScrollView(
                  child: Column(
                    children: [
                      Image(image: AssetImage('assets/images/logo.png')),
                      SizedBox(height: 16),
                      Text(switch (state.formType) {
                        AuthFormType.signup => 'Регистрация',
                        AuthFormType.signin => 'Вход',
                      }, style: Theme.of(context).textTheme.headlineSmall),
                      SizedBox(height: 20),
                      AuthForm(),
                    ],
                  ),
                ),
              ),
            );
          },
        ),
      ),
    );
  }
}
