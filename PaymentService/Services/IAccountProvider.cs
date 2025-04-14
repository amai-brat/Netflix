using PaymentService.Models;

namespace PaymentService.Services;

public interface IAccountProvider
{
    Task<string?> GetAccountNumber(Card card);
    Task<string> GetOrganizationAccountNumber();
}