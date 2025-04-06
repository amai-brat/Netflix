using PaymentService.Models;

namespace PaymentService.Services;

public class FakeAccountProvider : IAccountProvider
{
    private const int AccountNumberLength = 25;
    
    public Task<string?> GetAccountNumber(Card card)
    {
        var complement = new string('0', AccountNumberLength - card.CardNumber.Length);
        return Task.FromResult<string?>(card.CardNumber + complement);
    }

    public Task<string> GetOrganizationAccountNumber()
    {
        return Task.FromResult("1234567890123456789012345");
    }
}