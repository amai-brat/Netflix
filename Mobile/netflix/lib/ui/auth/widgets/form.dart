import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_state.dart';

class Form extends StatelessWidget {
  const Form({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<AuthBloc, AuthState>(builder: builder)
  }
}