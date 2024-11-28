using Bank.Server.Shared.Helpers;
using Bank.Server.Shared.Requests.Transactions;

namespace Bank.Server.Business.Abstractions;

public interface ITransactionService
{
    Task<Result<bool>> PerformTransactionAsync(long userId, PerformTransactionRequest request, CancellationToken cancellationToken = default);
}
