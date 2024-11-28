using Bank.Server.Business.Abstractions;
using Bank.Server.Core.Entities;
using Bank.Server.Persistence;
using Bank.Server.Shared.Constants;
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
                if (senderAccount is null)
                {
                    return new Result<bool>(false, ErrorMessages.Account.NotFound);
                }

                if (senderAccount.Balance < request.Amount)
                {
                    return new Result<bool>(false, ErrorMessages.Account.NotEnoughBalance);
                }

                senderAccount.Balance -= request.Amount;
                break;
            case Core.Enums.TransactionType.Deposit:
                if (receiverAccount is null)
                {
                    return new Result<bool>(false, ErrorMessages.Account.NotFound);
                }

                receiverAccount.Balance += request.Amount;
                break;
            case Core.Enums.TransactionType.Transfer:
                if (senderAccount is null)
                {
                    return new Result<bool>(false, ErrorMessages.Account.NotFound);
                }

                if (receiverAccount is null)
                {
                    return new Result<bool>(false, ErrorMessages.Account.NotFound);
                }

                if (senderAccount.Balance < request.Amount)
                {
                    return new Result<bool>(false, ErrorMessages.Account.NotEnoughBalance);
                }

                if (request.SenderAccountId == request.ReceiverAccountId)
                {
                    return new Result<bool>(false, ErrorMessages.Transaction.SameAccounts);
                }

                senderAccount.Balance -= request.Amount;
                receiverAccount.Balance += request.Amount;
                break;
            default:
                return new Result<bool>(false, ErrorMessages.Transaction.InvalidType);
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
