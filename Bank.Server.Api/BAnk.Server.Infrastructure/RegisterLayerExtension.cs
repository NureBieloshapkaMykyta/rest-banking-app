using Bank.Server.Business.Abstractions;
using Bank.Server.Infrastructure.Services;
using Bank.Server.Shared.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Bank.Server.Infrastructure;

public static class RegisterLayerExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddJwt(configuration);
        services.AddScoped<ITokenService, JwtService>();
    }

    private static void AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtIssuer = configuration.GetSection($"{JwtOptions.SectionName}:Issuer").Get<string>();
        var jwtKey = configuration.GetSection($"{JwtOptions.SectionName}:Key").Get<string>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
         .AddJwtBearer(options =>
         {
             options.TokenValidationParameters = new TokenValidationParameters
             {
                 ValidateIssuer = true,
                 ValidateAudience = true,
                 ValidateLifetime = true,
                 ValidateIssuerSigningKey = true,
                 ValidIssuer = jwtIssuer,
                 ValidAudience = jwtIssuer,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
             };
         });
    }
}
