using Microsoft.EntityFrameworkCore;
using PaymentService.Data.Abstractions;
using PaymentService.Models;

namespace PaymentService.Data.Impls;

public class CurrencyRepository(
    AppDbContext dbContext) : ICurrencyRepository
{
    public async Task<Currency?> GetCurrencyAsync(string currencyCode)
    {
        return await dbContext.Currencies.SingleOrDefaultAsync(x => x.Name == currencyCode);
    }
}