import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/repositories/auth_repository.dart';
import 'package:netflix/domain/use_cases/signin_use_case.dart';
import 'package:netflix/domain/use_cases/signout_use_case.dart';
import 'package:netflix/domain/use_cases/signup_use_case.dart';
import 'package:netflix/ui/core/bloc/user/user_bloc.dart';
import 'package:netflix/ui/main/main_page.dart';
import 'package:netflix/ui/profile/profile_page.dart';
import 'package:netflix/ui/profile/tabs/personal_info/personal_info_view.dart';
import 'package:netflix/ui/search/search_page.dart';
import 'package:netflix/utils/di.dart';
import 'package:netflix/utils/routes.dart';

class NetflixApp extends StatelessWidget {
  const NetflixApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider(
          create:
              (context) => UserBloc(
                authRepository: locator<AuthRepository>(),
                signUpUseCase: locator<SignUpUseCase>(),
                signInUseCase: locator<SignInUseCase>(),
                signOutUseCase: locator<SignOutUseCase>(),
              ),
        ),
      ],
      child: MaterialApp(
        theme: ThemeData(
          colorScheme: ColorScheme.dark(
            primary: Color(0xFFE50914),
            secondary: Color(0xFFE50914),
            surface: Color(0xFF000000),
          ),
          elevatedButtonTheme: ElevatedButtonThemeData(
            style: ElevatedButton.styleFrom(
              backgroundColor: Color(0xFFE50914),
              foregroundColor: Color(0xFFFFFFFF),
            ),
          ),
          inputDecorationTheme: InputDecorationTheme(
            fillColor: Colors.grey[900],
            filled: true,
            hintStyle: TextStyle(color: Colors.grey[400]),
            border: OutlineInputBorder(
              borderRadius: BorderRadius.all(Radius.circular(25)),
              borderSide: BorderSide.none,
            ),
          ),
        ),
        initialRoute: '/',
        routes: {
          Routes.main: (context) => const NetflixAppView(),
          Routes.profilePersonal: (context) => const PersonalInfoView()
        },
      ),
    );
  }
}

class NetflixAppView extends StatefulWidget {
  const NetflixAppView({super.key});

  @override
  State<StatefulWidget> createState() => _NetflixAppViewState();
}

class _NetflixAppViewState extends State<NetflixAppView> {
  int _idx = 0;
  final List<Widget> _pages = [MainPage(), SearchPage(), ProfilePage()];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _idx,
        onTap: (index) => setState(() => _idx = index),
        items: const [
          BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Главная'),
          BottomNavigationBarItem(icon: Icon(Icons.search), label: 'Поиск'),
          BottomNavigationBarItem(icon: Icon(Icons.person), label: 'Профиль'),
        ],
      ),
      body: _pages[_idx],
    );
  }
}
