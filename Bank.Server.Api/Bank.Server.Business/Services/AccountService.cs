using AutoMapper;
using Bank.Server.Business.Abstractions;
using Bank.Server.Core.Entities;
using Bank.Server.Persistence;
using Bank.Server.Shared.Helpers;
using Bank.Server.Shared.Options;
using Bank.Server.Shared.Requests.Account;
using Bank.Server.Shared.Responses.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Bank.Server.Business.Services;

public class AccountService(BankMasterDbContext dbContext, IMapper mapper, IOptions<AccountOptions> accountOptions) : IAccountService
{
    public async Task<Result<bool>> CreateAccountAsync(long userId, CancellationToken cancellationToken = default)
    {
        var initialAccount = new Account 
        { 
            Id = default,
            Balance = default,
            ActiveDue = DateTime.UtcNow.AddYears(accountOptions.Value.ValidityYears),
            UserId = userId,
            Number = Guid.NewGuid()
        };

        await dbContext.Accounts.AddAsync(initialAccount, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new Result<bool>(true);
    }

    public async Task<Result<AccountDetailsResponse>> GetAccountDetailsByNumberAsync(GetAccountDetailsRequest request, CancellationToken cancellationToken = default)
    {
        var account = await dbContext.Accounts
            .Include(x => x.TransactionsInitiated)
            .Include(x=>x.TransactionsIncoming)
            .FirstOrDefaultAsync(x => x.UserId==request.UserId && x.Number==request.Number, cancellationToken);

        if (account is null) 
        {
            return new Result<AccountDetailsResponse>(false, "");
        }

        return new Result<AccountDetailsResponse>(true, mapper.Map<AccountDetailsResponse>(account));
    }

    public async Task<Result<List<AccountResponse>>> GetUserAccountsAsync(long userId, CancellationToken cancellationToken = default)
    {
        var accounts = await dbContext.Accounts
            .Where(x => x.UserId == userId)
            .ToListAsync(cancellationToken);

        return new Result<List<AccountResponse>>(true, mapper.Map<List<AccountResponse>>(accounts));
    }
}
