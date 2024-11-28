namespace Bank.Server.Shared.Responses.Account;

public class AccountResponse
{
    public long Id { get; set; }
    public Guid Number { get; set; }
    public float Balance { get; set; }
    public DateTime ActiveDue { get; set; }
}
