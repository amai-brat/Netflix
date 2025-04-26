import 'package:flutter/material.dart';
import 'package:netflix/ui/main/main_page.dart';
import 'package:netflix/ui/profile/profile_page.dart';
import 'package:netflix/ui/search/search_page.dart';

class NetflixApp extends StatelessWidget {
  const NetflixApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      theme: ThemeData(
        colorScheme: ColorScheme.dark(
          primary: Color(0xFFE50914),
          secondary: Color(0xFFE50914),
          surface: Color(0xFF000000),
        ),
      ),
      initialRoute: '/',
      routes: {'/': (context) => const NetflixAppView()},
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
