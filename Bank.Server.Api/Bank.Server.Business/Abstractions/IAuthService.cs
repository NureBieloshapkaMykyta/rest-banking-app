using Bank.Server.Shared.Helpers;
using Bank.Server.Shared.Requests.Auth;

namespace Bank.Server.Business.Abstractions;

public interface IAuthService
{
    Task<Result<bool>> RegisterUserAsync(RegisterUserRequest request, CancellationToken cancellationToken = default);
    Task<Result<bool>> SignIn(SignInRequest request, CancellationToken cancellationToken = default);
}
