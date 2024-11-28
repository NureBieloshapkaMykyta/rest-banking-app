namespace Bank.Server.Business.Abstractions;

public interface ITokenService
{
    string GenerateToken(string token);
}
