using Microsoft.EntityFrameworkCore;

namespace Domain;

[Owned]
public class Budget
{
    public int BudgetValue { get; set; }
    public string BudgetCurrencyName { get; set; }  = null!;
}