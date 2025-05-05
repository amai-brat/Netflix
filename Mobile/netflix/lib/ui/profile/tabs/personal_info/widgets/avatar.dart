import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:image_picker/image_picker.dart';
import 'package:netflix/ui/profile/tabs/personal_info/bloc/personal_info_bloc.dart';

class AvatarWidget extends StatelessWidget {
  const AvatarWidget({super.key});

  Future<void> _changeAvatar(BuildContext context) async {
    final picker = ImagePicker();
    final pickedFile = await picker.pickImage(source: ImageSource.gallery);
    if (pickedFile != null) {
      // ignore: use_build_context_synchronously
      print("Фото сменено!");
    }
  }

  @override
  Widget build(BuildContext context) {
    return BlocBuilder<PersonalInfoBloc, PersonalInfoState>(
      builder: (context, state) {
        final userInfo = switch (state) {
          PersonalInfoLoaded(userInfo: final info) => info,
          _ => null,
        };

        if (userInfo == null) return const SizedBox.shrink();
        return GestureDetector(
          onLongPress: () => _changeAvatar(context),
          child: Center(
            child: SizedBox(
              width: 100,
              height: 100,
              child: ClipOval(
                child: userInfo.profilePictureUrl != null
                    ? Image.network(
                  userInfo.profilePictureUrl!,
                  fit: BoxFit.cover,
                  errorBuilder: (_, __, ___) => _buildDefaultAvatar(),
                ) : _buildDefaultAvatar(),
              ),
            ),
          ),
        );
      },
    );
  }

  Widget _buildDefaultAvatar() {
    return const Icon(
      Icons.person,
      size: 100,
      color: Colors.grey,
    );
  }
}