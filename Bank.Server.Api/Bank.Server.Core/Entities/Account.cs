namespace Bank.Server.Core.Entities;

public class Account
{
    public long Id { get; set; }
    public Guid Number { get; set; }
    public float Balance { get; set; }
    public DateTime ActiveDue { get; set; }
    public long UserId { get; set; }
    public virtual User? User { get; set; }
    public virtual ICollection<Transaction>? TransactionsInitiated { get; set; }
    public virtual ICollection<Transaction>? TransactionsIncoming { get; set; }
}
