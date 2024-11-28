using Bank.Server.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Bank.Server.Persistence.EntityConfigurations;

internal class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder
            .HasOne(x => x.Sender)
            .WithMany(x => x.TransactionsInitiated)
            .HasForeignKey(x => x.SenderId);

        builder
            .HasOne(x => x.Receiver)
            .WithMany(x => x.TransactionsIncoming)
            .HasForeignKey(x => x.ReceiverId);
    }
}

