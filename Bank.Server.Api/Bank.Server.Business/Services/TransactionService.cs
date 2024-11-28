using Bank.Server.Business.Abstractions;
using Bank.Server.Core.Entities;
using Bank.Server.Persistence;
using Bank.Server.Shared.Helpers;
using Bank.Server.Shared.Requests.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Bank.Server.Business.Services;

public class TransactionService(BankMasterDbContext dbContext) : ITransactionService
{
    public async Task<Result<bool>> PerformTransactionAsync(PerformTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var senderAccount = request.SenderAccountId is not null ?
            await dbContext.Accounts.FindAsync(request.SenderAccountId, cancellationToken)
            : null;
        var receiverAccount = request.ReceiverAccountId is not null ?
            await dbContext.Accounts.FindAsync(request.ReceiverAccountId, cancellationToken)
            : null;
        switch (request.Type)
        {
            case Core.Enums.TransactionType.Withdraw:
                senderAccount.Balance -= request.Amount;
                break;
            case Core.Enums.TransactionType.Deposit:
                receiverAccount.Balance += request.Amount;
                break;
            case Core.Enums.TransactionType.Transfer:
                senderAccount.Balance -= request.Amount;
                receiverAccount.Balance += request.Amount;
                break;
            default:
                return new Result<bool>(false, "");
        }

        var performedTransaction = new Transaction
        {
            Id = default,
            Amount = request.Amount,
            Date = DateTime.UtcNow,
            Type = request.Type,
            ReceiverId = request.ReceiverAccountId == 0 ? null : request.ReceiverAccountId,
            SenderId = request.SenderAccountId == 0 ? null : request.SenderAccountId,
        };

        await dbContext.Transactions.AddAsync(performedTransaction, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Result<bool>(true);
    }
}
