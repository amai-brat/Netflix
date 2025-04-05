using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PaymentService.Models;

namespace PaymentService.Data.Configurations;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasOne(x => x.Currency)
            .WithMany()
            .HasForeignKey(x => x.CurrencyId);
        
        builder.Property(x => x.AccountNumberFrom)
            .HasMaxLength(25);
        
        builder.Property(x => x.AccountNumberTo)
            .HasMaxLength(25);
    }
}