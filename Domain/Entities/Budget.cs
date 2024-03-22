using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Owned]
public class Budget
{
    public int BudgetValue { get; set; }
    public string BudgetCurrencyName { get; set; } = null!;
}