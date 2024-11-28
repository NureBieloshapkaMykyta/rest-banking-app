using Bank.Server.Shared.Helpers;
using Bank.Server.Shared.Requests.Account;
using Bank.Server.Shared.Responses.Account;

namespace Bank.Server.Business.Abstractions;

public interface IAccountService
{
    Task<Result<bool>> CreateAccountAsync(long userId, CancellationToken cancellationToken = default);
    Task<Result<AccountDetailsResponse>> GetAccountDetailsByNumberAsync(GetAccountDetailsRequest request, CancellationToken cancellationToken = default);
    Task<Result<List<AccountResponse>>> GetUserAccountsAsync(long userId, CancellationToken cancellationToken = default);
}
