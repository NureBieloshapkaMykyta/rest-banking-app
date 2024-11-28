using Bank.Server.Core.Enums;

namespace Bank.Server.Shared.Requests.Transactions;

public class PerformTransactionRequest
{
    public long? ReceiverId { get; set; }

    public float Amount { get; set; }

    public TransactionType Type { get; set; }
}
