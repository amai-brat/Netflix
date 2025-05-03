import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:netflix/domain/models/subscription.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_bloc.dart';
import 'package:netflix/ui/subscriptions/bloc/subscriptions_event.dart';

class BankCardForm extends StatefulWidget {
  final Subscription subscription;

  const BankCardForm({super.key, required this.subscription});

  String? cardNumberValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (value.length != 16) {
      return 'Номер карты состоит из 16 цифр';
    }
    return null;
  }

  String? cardOwnerValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    }
    return null;
  }

  String? validThruValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (!RegExp(r'^(0[1-9]|1[0-2])/[0-9]{2}$').hasMatch(value)) {
      return 'Дата в формате: ММ/ГГ';
    }
    return null;
  }

  String? cvcValidator(String? value) {
    if (value == null || value.isEmpty) {
      return 'Обязательное поле';
    } else if (!RegExp(r'^[0-9]{3}$').hasMatch(value)) {
      return '3 цифры на задней стороне карты';
    }
    return null;
  }

  @override
  State<StatefulWidget> createState() => _BankCardFormState();
}

class _BankCardFormState extends State<BankCardForm> {
  final _formKey = GlobalKey<FormState>();
  final _cardNumCtrl = TextEditingController();
  final _cardOwnerCtrl = TextEditingController();
  final _validThruCtrl = TextEditingController();
  final _cvcCtrl = TextEditingController();

  @override
  void dispose() {
    _cardNumCtrl.dispose();
    _cardOwnerCtrl.dispose();
    _validThruCtrl.dispose();
    _cvcCtrl.dispose();

    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      backgroundColor: Colors.black87,
      title: Text('Покупка подписки: ${widget.subscription.name}'),
      content: Form(
        key: _formKey,
        child: Container(
          constraints: BoxConstraints(maxWidth: 400),
          child: Column(
            spacing: 15,
            mainAxisSize: MainAxisSize.min,
            children: [
              TextFormField(
                controller: _cardNumCtrl,
                validator: widget.cardNumberValidator,
                decoration: const InputDecoration(labelText: 'Номер карты'),
                inputFormatters: [FilteringTextInputFormatter.digitsOnly],
              ),
              TextFormField(
                controller: _cardOwnerCtrl,
                validator: widget.cardOwnerValidator,
                decoration: const InputDecoration(
                  labelText: 'ФИО обладателя карты',
                ),
              ),
              Row(
                children: [
                  Expanded(
                    flex: 1,
                    child: TextFormField(
                      controller: _validThruCtrl,
                      validator: widget.validThruValidator,
                      decoration: const InputDecoration(labelText: 'Срок', errorMaxLines: 3),
                    ),
                  ),
                  const SizedBox(width: 15),
                  Expanded(
                    flex: 1,
                    child: TextFormField(
                      controller: _cvcCtrl,
                      validator: widget.cvcValidator,
                      decoration: InputDecoration(labelText: 'CVC/CVV', errorMaxLines: 3),
                      inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                    ),
                  ),
                ],
              ),
            ],
          ),
        ),
      ),
      actions: [
        ElevatedButton(
          onPressed: () {
            Navigator.of(context).pop();
          },
          child: const Text('Отмена'),
        ),
        ElevatedButton(
          onPressed: () async {
            if (_formKey.currentState!.validate()) {
              context.read<SubscriptionsBloc>().add(
                PurchasePressed(
                  subscriptionId: widget.subscription.id,
                  cardNumber: _cardNumCtrl.text,
                  cardOwner: _cardOwnerCtrl.text,
                  validThru: _validThruCtrl.text,
                  cvc: int.parse(_cvcCtrl.text),
                ),
              );

              await Future.delayed(Duration(seconds: 1));
              if (context.mounted) {
                Navigator.of(context).pop();
              }
            }
          },
          child: const Text('Купить'),
        ),
      ],
    );
  }
}
