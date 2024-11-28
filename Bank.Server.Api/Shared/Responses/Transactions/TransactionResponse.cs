using Bank.Server.Core.Enums;

namespace Bank.Server.Shared.Responses.Transactions;

public class TransactionResponse
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public TransactionType Type { get; set; }
    public float Amount { get; set; }
    public long? SenderId { get; set; }
    public long? ReceiverId { get; set; }
}
