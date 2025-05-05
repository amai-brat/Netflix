import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/ui/profile/tabs/personal_info/bloc/personal_info_bloc.dart';
import 'package:netflix/ui/profile/tabs/personal_info/widgets/change_password_dialog.dart';

class PersonalInfoForm extends StatefulWidget {
  final PersonalInfoLoaded state;

  const PersonalInfoForm({super.key, required this.state});

  @override
  State<PersonalInfoForm> createState() => _PersonalInfoFormState();
}

class _PersonalInfoFormState extends State<PersonalInfoForm> {
  late final TextEditingController _emailController;
  late final TextEditingController _birthDateController;

  @override
  void initState() {
    super.initState();
    _emailController = TextEditingController(text: widget.state.userInfo.email);
    _birthDateController = TextEditingController(text: widget.state.userInfo.birthDate);
  }

  @override
  void dispose() {
    _emailController.dispose();
    _birthDateController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        _buildReadOnlyField('Никнейм', widget.state.userInfo.nickname),
        const SizedBox(height: 16),
        _buildEditableField(
          context,
          'Email',
          _emailController,
              (value) => context.read<PersonalInfoBloc>().add(
            ChangeEmailEvent(value),
          ),
          widget.state.isEmailUpdating,
          widget.state.emailError,
          widget.state.emailUpdateSuccess,
        ),
        const SizedBox(height: 16),
        _buildEditableField(
          context,
          'Дата рождения',
          _birthDateController,
              (value) => context.read<PersonalInfoBloc>().add(
            ChangeBirthDateEvent(value),
          ),
          widget.state.isBirthDateUpdating,
          widget.state.birthDateError,
          widget.state.birthDateUpdateSuccess,
        ),
        const SizedBox(height: 24),
        _buildPasswordChangeSection(context),
      ],
    );
  }

  Widget _buildReadOnlyField(String label, String value) {
    return TextFormField(
      controller: TextEditingController(text: value),
      readOnly: true,
      decoration: InputDecoration(
        labelText: label,
        border: const OutlineInputBorder(),
        filled: true,
        fillColor: Theme.of(context).disabledColor.withValues(alpha: 0.1),
      ),
      style: Theme.of(context).textTheme.bodyMedium,
    );
  }

  Widget _buildEditableField(
      BuildContext context,
      String label,
      TextEditingController controller,
      Function(String) onChanged,
      bool isLoading,
      String? error,
      bool? success,
      ) {
    final isDateField = label == 'Дата рождения';
    final successMessage = isDateField ? 'Дата рождения изменена' : 'Email изменен';

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Stack(
          alignment: Alignment.centerRight,
          children: [
            TextFormField(
              controller: controller,
              decoration: InputDecoration(
                labelText: label,
                border: const OutlineInputBorder(),
                errorText: error,
                errorStyle: TextStyle(
                  color: Theme.of(context).colorScheme.error,
                ),
                suffixIcon: isLoading
                    ? const Padding(
                  padding: EdgeInsets.all(12),
                  child: CircularProgressIndicator(strokeWidth: 2),
                )
                    : IconButton(
                  icon: const Icon(Icons.save),
                  onPressed: () => onChanged(controller.text),
                ),
              ),
              onChanged: (value) => setState(() {}),
            ),
          ],
        ),
        if (success == true)
          Padding(
            padding: const EdgeInsets.only(top: 4),
            child: Text(
              successMessage,
              style: TextStyle(
                color: Colors.green,
                fontSize: 12,
              ),
            ),
          ),
        if (error != null)
          Padding(
            padding: const EdgeInsets.only(top: 4),
            child: Text(
              error,
              style: TextStyle(
                color: Theme.of(context).colorScheme.error,
                fontSize: 12,
              ),
            ),
          ),
      ],
    );
  }

  Widget _buildPasswordChangeSection(BuildContext context) {
    return Column(
      children: [
        const Divider(),
        ListTile(
          leading: const Icon(Icons.lock),
          title: const Text('Сменить пароль'),
          trailing: const Icon(Icons.chevron_right),
          onTap: () => _showChangePasswordDialog(context),
        ),
      ],
    );
  }

  void _showChangePasswordDialog(BuildContext context) {
    final bloc = context.read<PersonalInfoBloc>();

    showDialog(
      context: context,
      builder: (context) => BlocProvider.value(
        value: bloc,
        child: const ChangePasswordDialog(),
      ),
    );
  }
}