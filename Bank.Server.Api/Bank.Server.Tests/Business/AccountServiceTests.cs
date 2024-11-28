using AutoMapper;
using Bank.Server.Business.Services;
using Bank.Server.Core.Entities;
using Bank.Server.Persistence;
using Bank.Server.Shared.Constants;
using Bank.Server.Shared.Options;
using Bank.Server.Shared.Requests.Account;
using Bank.Server.Shared.Responses.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace Bank.Server.Tests.Business;

public class AccountServiceTests
{
    private readonly DbContextOptions<BankMasterDbContext> _dbContextOptions;
    private readonly IMapper _mapper;
    private readonly Mock<IOptions<AccountOptions>> _accountOptionsMock;

    public AccountServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BankMasterDbContext>()
            .UseInMemoryDatabase(databaseName: "AccountServiceTestDb")
            .Options;

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Account, AccountDetailsResponse>();
            cfg.CreateMap<Account, AccountResponse>();
        });
        _mapper = mapperConfig.CreateMapper();

        _accountOptionsMock = new Mock<IOptions<AccountOptions>>();
        _accountOptionsMock.Setup(x => x.Value).Returns(new AccountOptions { ValidityYears = 5 });
    }

    [Fact]
    public async Task CreateAccountAsync_ShouldCreateAccountSuccessfully()
    {
        long userId = 1;

        using var context = new BankMasterDbContext(_dbContextOptions);
        var accountService = new AccountService(context, _mapper, _accountOptionsMock.Object);

        var result = await accountService.CreateAccountAsync(userId);

        Assert.True(result.IsSuccessful);

        var createdAccount = await context.Accounts.FirstOrDefaultAsync(x => x.UserId == userId);
        Assert.NotNull(createdAccount);
        Assert.Equal(userId, createdAccount.UserId);
        Assert.Equal(0, createdAccount.Balance);
        Assert.True((createdAccount.ActiveDue - DateTime.UtcNow).TotalDays > 365 * 4);
    }

    [Fact]
    public async Task GetAccountDetailsByNumberAsync_ShouldReturnAccountDetails()
    {
        long userId = 2;
        var account = new Account
        {
            Id = 1,
            UserId = userId,
            Number = Guid.NewGuid(),
            Balance = 1000,
            ActiveDue = DateTime.UtcNow.AddYears(5)
        };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Accounts.Add(account);
            await context.SaveChangesAsync();
        }

        var request = new GetAccountDetailsRequest(userId, account.Number);

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        var accountService = new AccountService(newContext, _mapper, _accountOptionsMock.Object);

        var result = await accountService.GetAccountDetailsByNumberAsync(request);

        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Equal(account.Number, result.Data.Number);
        Assert.Equal(account.Balance, result.Data.Balance);
    }

    [Fact]
    public async Task GetAccountDetailsByNumberAsync_ShouldReturnNotFoundIfAccountDoesNotExist()
    {
        var request = new GetAccountDetailsRequest(3, Guid.NewGuid());

        using var context = new BankMasterDbContext(_dbContextOptions);
        var accountService = new AccountService(context, _mapper, _accountOptionsMock.Object);

        var result = await accountService.GetAccountDetailsByNumberAsync(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.Account.NotFound, result.Message);
    }

    [Fact]
    public async Task GetUserAccountsAsync_ShouldReturnUserAccounts()
    {
        long userId = 4;
        var accounts = new List<Account>
        {
            new Account { Id = 2, UserId = userId, Balance = 1000, Number = Guid.NewGuid() },
            new Account { Id = 3, UserId = userId, Balance = 2000, Number = Guid.NewGuid() },
        };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Accounts.AddRange(accounts);
            await context.SaveChangesAsync();
        }

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        var accountService = new AccountService(newContext, _mapper, _accountOptionsMock.Object);

        var result = await accountService.GetUserAccountsAsync(userId);

        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetUserAccountsAsync_ShouldReturnEmptyListIfNoAccounts()
    {
        long userId = 1;

        using var context = new BankMasterDbContext(_dbContextOptions);
        var accountService = new AccountService(context, _mapper, _accountOptionsMock.Object);

        var result = await accountService.GetUserAccountsAsync(userId);

        Assert.True(result.IsSuccessful);
        Assert.NotNull(result.Data);
        Assert.Empty(result.Data);
    }
}