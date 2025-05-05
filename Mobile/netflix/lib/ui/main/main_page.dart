import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/main/bloc/main_page_bloc.dart';
import 'package:netflix/ui/main/bloc/main_page_event.dart';
import 'package:netflix/ui/main/bloc/main_page_state.dart';
import 'package:netflix/ui/main/widgets/section_widget.dart';

class MainPage extends StatelessWidget {
  const MainPage({super.key});

  @override
  Widget build(BuildContext context) {
    return BlocProvider(
      create:
          (context) => MainPageBloc.createViaLocator()..add(MainPageOpened()),
      child: Scaffold(
        appBar: AppBar(title: const Text('Главная')),
        body: BlocBuilder<MainPageBloc, MainPageState>(
          builder: (context, state) {
            if (state.status == MainPageStatus.loading) {
              return Center(child: CircularProgressIndicator());
            }

            return ListView(
              children:
                  state.sections.map((s) => SectionWidget(section: s)).toList(),
            );
          },
        ),
      ),
    );
  }
}
