namespace PaymentService.Models;

public enum ReasonType : byte
{
    None = 0,
    Subscription = 1,
    Other = 255
}

public enum TransactionStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2,
    Rejected = 3,
    Refaunded = 10
}

public class Transaction
{
    public TransactionStatus Status { get; set; }
    
    public Guid Id { get; set; }
    public long UserId { get; set; }
    public DateTime Time { get; set; }
    
    public string AccountNumberFrom { get; set; } = null!;
    public string AccountNumberTo { get; set; } = null!;

    public ReasonType Reason { get; set; }

    public int CurrencyId { get; set; }
    public Currency Currency { get; set; } = null!;
    
    public decimal Amount { get; set; }

    public static Transaction Create(
        long userId,
        string accountNumberFrom,
        string accountNumberTo,
        ReasonType reason,
        Currency currency,
        decimal amount,
        TransactionStatus status = TransactionStatus.Pending)
    {
        return new Transaction
        {
            Status = status,
            Id = Guid.NewGuid(),
            UserId = userId,
            Time = DateTime.UtcNow,
            AccountNumberFrom = accountNumberFrom,
            AccountNumberTo = accountNumberTo,
            Reason = reason,
            Currency = currency,
            Amount = amount,
        };
    }
}