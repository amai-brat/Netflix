import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
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
  bool _emailUpdated = false;

  String? emailValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (!RegExp(r'^[\w\.-]+@[\w\.-]+\.\w+$').hasMatch(value)) {
      return 'Некорректный email';
    }
    return null;
  }

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
        _buildEmailField(context),
        const SizedBox(height: 16),
        _buildDatePickerField(context),
        const SizedBox(height: 24),
        _buildPasswordChangeSection(context),
      ],
    );
  }


  Widget _buildDatePickerField(BuildContext context) {
    final success = widget.state.birthDateUpdateSuccess;
    final error = widget.state.birthDateError;

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        TextFormField(
          controller: _birthDateController,
          keyboardType: TextInputType.number,
          decoration: InputDecoration(
            labelText: 'Дата рождения (ДД.MM.ГГГГ)',
            border: const OutlineInputBorder(),
            suffixIcon: widget.state.isBirthDateUpdating
                ? const Padding(
              padding: EdgeInsets.all(12),
              child: CircularProgressIndicator(strokeWidth: 2),
            )
                : IconButton(
              icon: const Icon(Icons.save),
              onPressed: () {
                final text = _birthDateController.text.trim();
                final parsed = DateTime.tryParse(text.replaceAll('.', '-'));
                if (parsed != null) {
                  context.read<PersonalInfoBloc>().add(ChangeBirthDateEvent(text));
                } else {
                  context.read<PersonalInfoBloc>().add(
                    ChangeBirthDateEvent('invalid-date'),
                  );
                }
              },
            ),
            errorText: error,
          ),
          inputFormatters: [
            _DateInputFormatter(),
          ],
        ),
        if (success == true)
          const Padding(
            padding: EdgeInsets.only(top: 4),
            child: Text(
              'Дата рождения изменена',
              style: TextStyle(color: Colors.green, fontSize: 12),
            ),
          ),
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

  Widget _buildEmailField(BuildContext context) {
    final success = widget.state.emailUpdateSuccess;
    final error = widget.state.emailError;

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        TextFormField(
          controller: _emailController,
          decoration: InputDecoration(
            labelText: 'Email',
            border: const OutlineInputBorder(),
            suffixIcon: widget.state.isEmailUpdating
                ? const Padding(
              padding: EdgeInsets.all(12),
              child: CircularProgressIndicator(strokeWidth: 2),
            )
                : IconButton(
              icon: const Icon(Icons.save),
              onPressed: () {
                final validationError = emailValidator(_emailController.text);
                if (validationError == null) {
                  context.read<PersonalInfoBloc>().add(
                    ChangeEmailEvent(_emailController.text),
                  );
                  setState(() {
                    _emailUpdated = true;
                  });
                } else {
                  setState(() {}); // force rebuild to show inline error
                }
              },
            ),
            errorText: emailValidator(_emailController.text) ?? error,
          ),
          onChanged: (value) {
            if (_emailUpdated) {
              setState(() {
                _emailUpdated = false;
              });
            }
          },
        ),
        if (success == true && _emailUpdated)
          const Padding(
            padding: EdgeInsets.only(top: 4),
            child: Text(
              'Email изменен',
              style: TextStyle(color: Colors.green, fontSize: 12),
            ),
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

class _DateInputFormatter extends TextInputFormatter {
  static const _template = '00.00.0000';
  static final _digitIndices = [0, 1, 3, 4, 6, 7, 8, 9]; // позиции для цифр
  static final _separatorIndices = [2, 5]; // позиции точек

  @override
  TextEditingValue formatEditUpdate(
      TextEditingValue oldValue, TextEditingValue newValue) {

    String currentText = oldValue.text;
    int cursorPosition = oldValue.selection.baseOffset;

    // Инициализация поля при пустом значении
    if (currentText.isEmpty || currentText.length != _template.length) {
      currentText = _template;
    }

    // Обработка удаления (backspace)
    if (newValue.text.length < oldValue.text.length) {
      int deletePosition = cursorPosition - 1;

      // Если удаляем цифры
      if (_digitIndices.contains(deletePosition)) {
        final chars = currentText.split('');
        chars[deletePosition] = '0'; // Заменяем цифру на 0
        currentText = chars.join('');
        cursorPosition = deletePosition;
      } else if (_separatorIndices.contains(deletePosition)) {
        // Если удаляем точку, то курсор остаётся на месте
        cursorPosition = deletePosition;
      }
    }
    // Обработка ввода (цифры)
    else if (newValue.text.length > oldValue.text.length) {
      int inputPosition = cursorPosition;

      // Пропускаем разделители
      while (inputPosition < _template.length &&
          !_digitIndices.contains(inputPosition)) {
        inputPosition++;
      }

      if (inputPosition < _template.length) {
        // Берем последнюю введенную цифру
        String newDigit = newValue.text[cursorPosition];

        if (RegExp(r'\d').hasMatch(newDigit)) {
          final chars = currentText.split('');
          // Заменяем цифру на текущей позиции
          chars[inputPosition] = newDigit;
          currentText = chars.join('');

          // Перемещаем курсор на 1 вправо
          cursorPosition = inputPosition + 1;
          while (cursorPosition < _template.length &&
              !_digitIndices.contains(cursorPosition)) {
            cursorPosition++;
          }
        }
      }
    }

    return TextEditingValue(
      text: currentText,
      selection: TextSelection.collapsed(offset: cursorPosition),
    );
  }
}