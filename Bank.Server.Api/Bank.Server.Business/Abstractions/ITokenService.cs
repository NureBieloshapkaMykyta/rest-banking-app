using Bank.Server.Core.Entities;

namespace Bank.Server.Business.Abstractions;

public interface ITokenService
{
    string GenerateToken(User user);
}
