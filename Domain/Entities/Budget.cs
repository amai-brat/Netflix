using Microsoft.EntityFrameworkCore;
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Domain.Entities;

[Owned]
public class Budget
{
    public int BudgetValue { get; set; }
    public string BudgetCurrencyName { get; set; } = null!;
}