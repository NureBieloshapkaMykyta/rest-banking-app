using Bank.Server.Business.Abstractions;
using Bank.Server.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Server.Business;

public static class RegisterLayerExtension
{
    public static void AddBusiness(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<ITransactionService, TransactionService>();
    }
}
