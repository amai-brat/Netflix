import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:go_router/go_router.dart';
import 'package:netflix/ui/core/cubit/navigation/navigation_cubit.dart';

import '../../../utils/routes.dart';

class NetflixAppView extends StatelessWidget {
  final Widget screen;

  NetflixAppView({super.key, required this.screen});

  final tabs = [
    BottomNavigationBarItem(icon: Icon(Icons.home), label: 'Главная'),
    BottomNavigationBarItem(icon: Icon(Icons.search), label: 'Поиск'),
    BottomNavigationBarItem(icon: Icon(Icons.person), label: 'Профиль'),
  ];

  String _switchBarItemIndexToRoute(int index) {
    return switch (index) {
      0 => Routes.main,
      1 => Routes.search,
      2 => Routes.profile,
      int() => throw Exception('Bad navigation bar index'),
    };
  }

  BlocBuilder<NavigationCubit, NavigationState> _buildBottomNavigation(
    List<BottomNavigationBarItem> tabs,
  ) => BlocBuilder<NavigationCubit, NavigationState>(
    buildWhen: (previous, current) => previous.index != current.index,
    builder: (context, state) {
      return BottomNavigationBar(
        onTap: (value) {
          if (state.index != value) {
            context.read<NavigationCubit>().getNavBarItem(value);
            context.go(_switchBarItemIndexToRoute(value));
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
      body: screen,
      bottomNavigationBar: _buildBottomNavigation(tabs),
    );
  }
}
