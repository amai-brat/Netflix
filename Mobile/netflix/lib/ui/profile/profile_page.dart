import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/auth/auth_page.dart';
import 'package:netflix/ui/core/bloc/user/user_bloc.dart';
import 'package:netflix/ui/core/bloc/user/user_event.dart';
import 'package:netflix/ui/core/bloc/user/user_state.dart';

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
        if (state.status == UserStatus.unauthenticated) {
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
            onTap: () => _navigateTo(context, '/personal'),
          ),
          ListTile(
            title: const Text('Избранное'),
            leading: const Icon(Icons.favorite),
            onTap: () => _navigateTo(context, '/favorites'),
          ),
          ListTile(
            title: const Text('Рецензии'),
            leading: const Icon(Icons.reviews),
            onTap: () => _navigateTo(context, '/reviews'),
          ),
          ListTile(
            title: const Text('Подписки'),
            leading: const Icon(Icons.subscriptions),
            onTap: () => _navigateTo(context, '/subscriptions'),
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

  void _navigateTo(BuildContext context, String route) {
    Navigator.pop(context); // Close drawer
    Navigator.pushNamed(context, route);
  }
}
