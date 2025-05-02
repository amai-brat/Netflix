import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:netflix/ui/core/cubit/navigation/navigation_cubit.dart';

class NetflixAppView extends StatelessWidget {
  final StatefulNavigationShell navigationShell;

  NetflixAppView({super.key, required this.navigationShell});

  final tabs = [
    BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Главная'),
    BottomNavigationBarItem(icon: Icon(Icons.search), label: 'Поиск'),
    BottomNavigationBarItem(icon: Icon(Icons.person), label: 'Профиль'),
  ];

  BlocBuilder<NavigationCubit, NavigationState> _buildBottomNavigation(
    List<BottomNavigationBarItem> tabs,
  ) => BlocBuilder<NavigationCubit, NavigationState>(
    buildWhen: (previous, current) => previous.index != current.index,
    builder: (context, state) {
      return BottomNavigationBar(
        onTap: (value) {
          if (state.index != value) {
            context.read<NavigationCubit>().getNavBarItem(value);
            navigationShell.goBranch(value);
          }
        },
        items: tabs,
        currentIndex: state.index,
      );
    },
  );

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: navigationShell,
      bottomNavigationBar: _buildBottomNavigation(tabs),
    );
  }
}
