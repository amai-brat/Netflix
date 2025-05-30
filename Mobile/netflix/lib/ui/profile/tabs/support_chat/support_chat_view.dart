import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/clients/grpc_support_chat_client.dart';
import 'package:netflix/domain/use_cases/support/get_history_use_case.dart';
import 'package:netflix/domain/use_cases/support/upload_files_use_case.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/bloc/support_chat_event.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_body.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/bloc/support_chat_control_bloc.dart';
import 'package:netflix/ui/profile/tabs/support_chat/components/support_chat_control/support_chat_control.dart';
import 'package:netflix/utils/app_colors.dart';
import 'package:netflix/utils/di.dart';

class SupportChatView extends StatelessWidget {
  const SupportChatView({super.key});

  @override
  Widget build(BuildContext context) {
    return MultiBlocProvider(
      providers: [
        BlocProvider(create: (context) => SupportChatBloc(
          client: locator<GrpcSupportChatClient>(),
          getHistoryUseCase: locator<GetHistoryUseCase>(),
          uploadFilesUseCase: locator<UploadFilesUseCase>(),
          )..add(ConnectSupportChatEvent()),
        ),
        BlocProvider(create: (context) => SupportChatControlBloc())
      ],
      child: Scaffold(
          appBar: AppBar(
            title: const Text('Чат с поддержкой'),
            elevation: 0,
          ),
          backgroundColor: AppColors.backgroundBlack,
          body: Column(
            children: [
              Expanded(child: SupportChatBody()),
              const SupportChatControl(),
            ],
          )
      ),
    );
  }
}