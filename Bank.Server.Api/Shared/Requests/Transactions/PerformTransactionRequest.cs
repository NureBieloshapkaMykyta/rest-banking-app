using Bank.Server.Core.Enums;

namespace Bank.Server.Shared.Requests.Transactions;

public class PerformTransactionRequest
{
    public long? SenderAccountId { get; set; }

    public long? ReceiverAccountId { get; set; }

    public float Amount { get; set; }

    public TransactionType Type { get; set; }
}
