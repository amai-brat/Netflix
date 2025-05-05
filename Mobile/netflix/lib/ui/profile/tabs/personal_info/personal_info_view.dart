import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/personal_info/bloc/personal_info_bloc.dart';
import 'package:netflix/ui/profile/tabs/personal_info/widgets/avatar.dart';
import 'package:netflix/ui/profile/tabs/personal_info/widgets/personal_info_form.dart';

class PersonalInfoView extends StatelessWidget {
  const PersonalInfoView({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Личные данные',
          overflow: TextOverflow.visible,
        ),
      ),
      body: BlocProvider(
        create: (context) => PersonalInfoBloc.createViaLocator()..add(LoadUserInfoEvent()),
        child: BlocConsumer<PersonalInfoBloc, PersonalInfoState>(
          listener: (context, state) {
            if (state is PersonalInfoError) {
              ScaffoldMessenger.of(context).showSnackBar(
                SnackBar(content: Text(state.message)),
              );
            }
          },
          builder: (context, state) {
            return switch (state) {
              PersonalInfoLoading() => const _LoadingIndicator(),
              PersonalInfoLoaded() => SingleChildScrollView(
                padding: const EdgeInsets.all(16),
                child: Column(
                  children: [
                    const AvatarWidget(),
                    const SizedBox(height: 24),
                    PersonalInfoForm(state: state),
                  ],
                ),
              ),
              PersonalInfoError() => const _ErrorWidget(),
            };
          },
        ),
      ),
    );
  }
}

class _LoadingIndicator extends StatelessWidget {
  const _LoadingIndicator();

  @override
  Widget build(BuildContext context) {
    return const Center(child: CircularProgressIndicator());
  }
}

class _ErrorWidget extends StatelessWidget {
  const _ErrorWidget();

  @override
  Widget build(BuildContext context) {
    return Center(
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        children: [
          const Text('Ошибка загрузки данных'),
          const SizedBox(height: 16),
          ElevatedButton(
            onPressed: () => context.read<PersonalInfoBloc>().add(LoadUserInfoEvent()),
            child: const Text('Повторить'),
          ),
        ],
      ),
    );
  }
}