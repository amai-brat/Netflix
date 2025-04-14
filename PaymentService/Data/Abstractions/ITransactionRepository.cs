using PaymentService.Models;

namespace PaymentService.Data.Abstractions;

public interface ITransactionRepository
{
    Task<Transaction> AddTransaction(Transaction transaction);
    Task<Transaction?> FindTransaction(Guid transactionId);
}