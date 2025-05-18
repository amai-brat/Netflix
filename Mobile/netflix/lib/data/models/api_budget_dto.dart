import 'package:netflix/domain/models/content/budget.dart';

class ApiBudgetDto {
  final int budgetValue;
  final String budgetCurrencyName;

  const ApiBudgetDto({
    required this.budgetValue,
    required this.budgetCurrencyName,
  });

  ApiBudgetDto.fromMap(Map<String, dynamic> map)
    : budgetValue = map['budgetValue'],
      budgetCurrencyName = map['budgetCurrencyName'];

  Budget toBudget() =>
      Budget(budgetValue: budgetValue, budgetCurrencyName: budgetCurrencyName);
}
