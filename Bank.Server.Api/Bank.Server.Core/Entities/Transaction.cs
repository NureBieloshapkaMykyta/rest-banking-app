using Bank.Server.Core.Enums;

namespace Bank.Server.Core.Entities;

public class Transaction
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public float Amount { get; set; }
    public long? SenderId { get; set; }
    public virtual Account? Sender { get; set; }
    public long? ReceiverId { get; set; }
    public virtual Account? Receiver { get; set; }
}
