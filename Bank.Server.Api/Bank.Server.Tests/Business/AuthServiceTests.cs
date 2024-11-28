using AutoMapper;
using Bank.Server.Business.Abstractions;
using Bank.Server.Business.Extensions;
using Bank.Server.Business.Services;
using Bank.Server.Core.Entities;
using Bank.Server.Persistence;
using Bank.Server.Shared.Constants;
using Bank.Server.Shared.Requests.Auth;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Bank.Server.Tests.Business;

public class AuthServiceTests
{
    private readonly DbContextOptions<BankMasterDbContext> _dbContextOptions;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ITokenService> _tokenServiceMock;

    public AuthServiceTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<BankMasterDbContext>()
            .UseInMemoryDatabase(databaseName: "AuthServiceTestDb")
            .Options;

        _mapperMock = new Mock<IMapper>();
        _tokenServiceMock = new Mock<ITokenService>();
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldAddUserAndReturnSuccess()
    {
        var request = new RegisterUserRequest("test@example.com","password123","Nikita", "Bieloshapka");

        using var context = new BankMasterDbContext(_dbContextOptions);
        _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("test-token");
        var authService = new AuthService(context, _tokenServiceMock.Object, _mapperMock.Object);

        var result = await authService.RegisterUserAsync(request);

        Assert.True(result.IsSuccessful);

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        Assert.NotNull(user);
        Assert.Equal(request.FirstName, user.FirstName);
        Assert.Equal(request.LastName, user.LastName);
        Assert.Equal(HashExtension.GetHash(request.Password), user.PasswordHash);
    }

    [Fact]
    public async Task SignIn_ShouldReturnTokenWhenCredentialsAreValid()
    {
        var request = new SignInRequest("test1@example.com", "password123");

        var user = new User
        {
            Id = 2,
            Email = request.Email,
            PasswordHash = HashExtension.GetHash(request.Password),
            FirstName = "Nikita",
            LastName = "Bieloshapka"
        };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("test-token");
        var authService = new AuthService(newContext, _tokenServiceMock.Object, _mapperMock.Object);

        var result = await authService.SignIn(request);

        Assert.True(result.IsSuccessful);
        Assert.Equal("test-token", result.Data);
    }

    [Fact]
    public async Task SignIn_ShouldReturnErrorWhenUserDoesNotExist()
    {
        var request = new SignInRequest("nonexistent@example.com", "password123");

        using var context = new BankMasterDbContext(_dbContextOptions);
        _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("test-token");
        var authService = new AuthService(context, _tokenServiceMock.Object, _mapperMock.Object);

        var result = await authService.SignIn(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.Auth.InvadidEmailOrPassword, result.Message);
    }

    [Fact]
    public async Task SignIn_ShouldReturnErrorWhenPasswordIsIncorrect()
    {
        var user = new User
        {
            Id = 3,
            Email = "test@example.com",
            PasswordHash = HashExtension.GetHash("correctpassword"),
            FirstName = "Nikita",
            LastName = "Bieloshapka"
        };

        using (var context = new BankMasterDbContext(_dbContextOptions))
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        var request = new SignInRequest("test2@example.com", "wrongpassword");

        using var newContext = new BankMasterDbContext(_dbContextOptions);
        _tokenServiceMock.Setup(x => x.GenerateToken(It.IsAny<User>())).Returns("test-token");
        var authService = new AuthService(newContext, _tokenServiceMock.Object, _mapperMock.Object);

        var result = await authService.SignIn(request);

        Assert.False(result.IsSuccessful);
        Assert.Equal(ErrorMessages.Auth.InvadidEmailOrPassword, result.Message);
    }
}
