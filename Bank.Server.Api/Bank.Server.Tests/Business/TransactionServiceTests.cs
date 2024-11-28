using Bank.Server.Business.Services;
using Bank.Server.Core.Entities;
using Bank.Server.Core.Enums;
using Bank.Server.Persistence;
using Bank.Server.Shared.Constants;
using Bank.Server.Shared.Requests.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Bank.Server.Tests.Business;

public class TransactionServiceTests
{
    private readonly DbContextOptions<BankMasterDbContext> _dbContextOptions;

    public TransactionServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BankMasterDbContext>()
            .UseInMemoryDatabase(databaseName: "TransactionServiceTestDb")
            .Options;
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldWithdrawSuccessfully()
    {
        var senderAccount = new Account { Id = 2, Balance = 1000 };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Accounts.Add(senderAccount);
            await context.SaveChangesAsync();
        }

        var request = new PerformTransactionRequest
        {
            SenderAccountId = senderAccount.Id,
            Amount = 200,
            Type = TransactionType.Withdraw
        };

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        var transactionService = new TransactionService(newContext);

        var result = await transactionService.PerformTransactionAsync(request);

        Assert.True(result.IsSuccessful);

        var updatedAccount = await newContext.Accounts.FindAsync(senderAccount.Id);
        Assert.Equal(800, updatedAccount!.Balance);

        var transaction = await newContext.Transactions.OrderBy(x=>x.Date).LastOrDefaultAsync();
        Assert.NotNull(transaction);
        Assert.Equal(200, transaction.Amount);
        Assert.Equal(TransactionType.Withdraw, transaction.Type);
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldFailWithdrawWhenInsufficientBalance()
    {
        var senderAccount = new Account { Id = 3, Balance = 100 };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Accounts.Add(senderAccount);
            await context.SaveChangesAsync();
        }

        var request = new PerformTransactionRequest
        {
            SenderAccountId = senderAccount.Id,
            Amount = 200,
            Type = TransactionType.Withdraw
        };

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        var transactionService = new TransactionService(newContext);

        var result = await transactionService.PerformTransactionAsync(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.Account.NotEnoughBalance, result.Message);

        var updatedAccount = await newContext.Accounts.FindAsync(senderAccount.Id);
        Assert.Equal(100, updatedAccount!.Balance);
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldDepositSuccessfully()
    {
        var receiverAccount = new Account { Id = 4, Balance = 500 };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Accounts.Add(receiverAccount);
            await context.SaveChangesAsync();
        }

        var request = new PerformTransactionRequest
        {
            ReceiverAccountId = receiverAccount.Id,
            Amount = 300,
            Type = TransactionType.Deposit
        };

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        var transactionService = new TransactionService(newContext);

        var result = await transactionService.PerformTransactionAsync(request);

        Assert.True(result.IsSuccessful);

        var updatedAccount = await newContext.Accounts.FindAsync(receiverAccount.Id);
        Assert.Equal(800, updatedAccount!.Balance);

        var transaction = await newContext.Transactions.OrderBy(x=>x.Date).LastOrDefaultAsync();
        Assert.NotNull(transaction);
        Assert.Equal(300, transaction.Amount);
        Assert.Equal(TransactionType.Deposit, transaction.Type);
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldTransferSuccessfully()
    {
        var senderAccount = new Account { Id = 5, Balance = 1000 };
        var receiverAccount = new Account { Id = 6, Balance = 500 };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Accounts.Add(senderAccount);
            context.Accounts.Add(receiverAccount);
            await context.SaveChangesAsync();
        }

        var request = new PerformTransactionRequest
        {
            SenderAccountId = senderAccount.Id,
            ReceiverAccountId = receiverAccount.Id,
            Amount = 300,
            Type = TransactionType.Transfer
        };

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        var transactionService = new TransactionService(newContext);

        var result = await transactionService.PerformTransactionAsync(request);

        Assert.True(result.IsSuccessful);

        var updatedSenderAccount = await newContext.Accounts.FindAsync(senderAccount.Id);
        var updatedReceiverAccount = await newContext.Accounts.FindAsync(receiverAccount.Id);
        Assert.Equal(700, updatedSenderAccount!.Balance);
        Assert.Equal(800, updatedReceiverAccount!.Balance);

        var transaction = await newContext.Transactions.FirstOrDefaultAsync();
        Assert.NotNull(transaction);
        Assert.Equal(300, transaction.Amount);
        Assert.Equal(TransactionType.Transfer, transaction.Type);
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldFailWhenSameAccountsForTransfer()
    {
        var senderAccount = new Account { Id = 7, Balance = 1000 };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Accounts.Add(senderAccount);
            await context.SaveChangesAsync();
        }

        var request = new PerformTransactionRequest
        {
            SenderAccountId = senderAccount.Id,
            ReceiverAccountId = senderAccount.Id,
            Amount = 300,
            Type = TransactionType.Transfer
        };

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        var transactionService = new TransactionService(newContext);

        var result = await transactionService.PerformTransactionAsync(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.Transaction.SameAccounts, result.Message);

        var updatedAccount = await newContext.Accounts.FindAsync(senderAccount.Id);
        Assert.Equal(1000, updatedAccount!.Balance);
    }

    [Fact]
    public async Task PerformTransactionAsync_ShouldFailWhenInvalidTransactionType()
    {
        var request = new PerformTransactionRequest
        {
            SenderAccountId = 8,
            ReceiverAccountId = 2,
            Amount = 100,
            Type = (TransactionType)999
        };

        using var context = new BankMasterDbContext(_dbContextOptions);
        var transactionService = new TransactionService(context);

        var result = await transactionService.PerformTransactionAsync(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.Transaction.InvalidType, result.Message);
    }
}
