import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/auth/bloc/auth_event.dart';
import 'package:netflix/ui/auth/bloc/auth_state.dart';

class AuthBloc extends Bloc<AuthEvent, AuthState> {
  AuthBloc(super.initialState);

}