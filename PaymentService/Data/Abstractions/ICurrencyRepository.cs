using PaymentService.Models;

namespace PaymentService.Data.Abstractions;

public interface ICurrencyRepository
{
    Task<Currency?> GetCurrencyAsync(string currencyCode);
}