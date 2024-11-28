using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bank.Server.Persistence;

public static class RegisterLayerExtension
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BankMasterDbContext>(options =>
        {
            options.UseNpgsql(
                    configuration.GetConnectionString("MasterDatabase"));
        });
    }
}
