import 'package:flutter/material.dart';

class ProfilePage extends StatelessWidget {
  const ProfilePage({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('Профиль')),
      endDrawer: Drawer(
        child: ListView(
          children: [
            ListTile(
              title: const Text('Личные данные'),
              leading: Icon(Icons.info),
            ),
            ListTile(
              title: const Text('Избранное'),
              leading: Icon(Icons.favorite),
            ),
            ListTile(
              title: const Text('Рецензии'),
              leading: Icon(Icons.reviews),
            ),
            ListTile(
              title: const Text('Подписки'),
              leading: Icon(Icons.subscriptions),
            ),
          ],
        ),
      ),
      body: const Center(child: Text('Страница профиля')),
    );
  }
}
