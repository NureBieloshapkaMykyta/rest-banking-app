namespace Bank.Server.Shared.Requests.Account;

public record GetAccountDetailsRequest(long UserId, Guid Number);
