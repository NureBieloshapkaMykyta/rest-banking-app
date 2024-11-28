using Bank.Server.Core.Enums;
using Bank.Server.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Bank.Server.Shared.Requests.Transactions;

public class PerformTransactionRequest
{
    public long? SenderAccountId { get; set; }
    public long? ReceiverAccountId { get; set; }

    [Range(0, float.MaxValue, ErrorMessage = $"{nameof(Amount)} must be greater that 0 and be valid float number")]
    public float Amount { get; set; }

    public TransactionType Type { get; set; }
}
