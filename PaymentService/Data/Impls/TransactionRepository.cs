using PaymentService.Data.Abstractions;
using PaymentService.Models;

namespace PaymentService.Data.Impls;

public class TransactionRepository(
    AppDbContext dbContext) : ITransactionRepository
{
    public async Task<Transaction> AddTransaction(Transaction transaction)
    {
        var entry = await dbContext.Transactions.AddAsync(transaction);
        return entry.Entity;
    }

    public async Task<Transaction?> FindTransaction(Guid transactionId)
    {
        return await dbContext.Transactions.FindAsync(transactionId);
    }
}