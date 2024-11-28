using Bank.Server.Shared.Responses.Transactions;

namespace Bank.Server.Shared.Responses.Account;

public class AccountDetailsResponse
{
    public long Id { get; set; }
    public Guid Number { get; set; }
    public float Balance { get; set; }
    public DateTime ActiveDue { get; set; }
    public List<TransactionResponse>? TransactionsInitiated { get; set; }
    public List<TransactionResponse>? TransactionsIncoming { get; set; }
}
