using AutoMapper;
using Bank.Server.Business.Abstractions;
using Bank.Server.Business.Extensions;
using Bank.Server.Core.Entities;
using Bank.Server.Persistence;
using Bank.Server.Shared.Helpers;
using Bank.Server.Shared.Requests.Auth;
using Microsoft.EntityFrameworkCore;

namespace Bank.Server.Business.Services;

public class AuthService(BankMasterDbContext dbContext, ITokenService tokenService, IMapper mapper) : IAuthService
{
    public async Task<Result<bool>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var initialUser = new User()
        {
            Id = default,
            Email = request.Email,
            PasswordHash = HashExtension.GetHash(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
        };

        await dbContext.Users.AddAsync(initialUser, cancellationToken);

        await dbContext.SaveChangesAsync();

        return new Result<bool>(true);
    }

    public async Task<Result<string>> SignIn(SignInRequest request, CancellationToken cancellationToken = default)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user is null || HashExtension.GetHash(request.Password) != user.PasswordHash) 
        {
            return new Result<string>(false, "");
        }

        return new Result<string>(true, data: tokenService.GenerateToken(user));
    }
}
