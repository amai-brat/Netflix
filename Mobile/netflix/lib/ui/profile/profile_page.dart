import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:netflix/ui/auth/auth_page.dart';
import 'package:netflix/ui/core/bloc/user/user_bloc.dart';
import 'package:netflix/ui/core/bloc/user/user_event.dart';
import 'package:netflix/ui/core/bloc/user/user_state.dart';
import 'package:netflix/utils/routes.dart';

class ProfilePage extends StatefulWidget {
  const ProfilePage({super.key});

  @override
  State<ProfilePage> createState() => _ProfilePageState();
}

class _ProfilePageState extends State<ProfilePage> {
  @override
  void initState() {
    super.initState();
    context.read<UserBloc>().add(AuthRequiredOpened());
  }

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<UserBloc, UserState>(
      builder: (context, state) {
        if (state.status == UserStatus.initial ||
            state.status == UserStatus.loading) {
          return const Scaffold(
            body: Center(child: CircularProgressIndicator()),
          );
        }
        if (state.status != UserStatus.authenticated) {
          return AuthPage();
        }

        return Scaffold(
          appBar: AppBar(title: const Text('Профиль')),
          endDrawer: _buildDrawer(context),
          body: const Center(child: Text('Страница профиля')),
        );
      },
    );
  }

  Widget _buildDrawer(BuildContext context) {
    return Drawer(
      child: ListView(
        children: [
          ListTile(
            title: const Text('Личные данные'),
            leading: const Icon(Icons.info),
            onTap: () => _goTo(context, Routes.profilePersonal),
          ),
          ListTile(
            title: const Text('Избранное'),
            leading: const Icon(Icons.favorite),
            onTap: () => _goTo(context, Routes.profileFavorites),
          ),
          ListTile(
            title: const Text('Рецензии'),
            leading: const Icon(Icons.reviews),
            onTap: () => _goTo(context, Routes.profileReviews),
          ),
          ListTile(
            title: const Text('Подписки'),
            leading: const Icon(Icons.subscriptions),
            onTap: () => _goTo(context, Routes.profileSubscriptions),
          ),
          const Divider(),
          ListTile(
            title: const Text('Выйти'),
            leading: const Icon(Icons.logout),
            onTap: () => context.read<UserBloc>().add(SignOutPressed()),
          ),
        ],
      ),
    );
  }

  void _goTo(BuildContext context, String route) {
    Navigator.of(context).pop(); // Close drawer
    context.go(route);
  }
}
