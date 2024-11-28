using System.Security.Claims;

namespace Bank.Server.Api.Extensions;

public static class PrincipalExtension
{
    public static string GetValueByClaimType(this ClaimsPrincipal principal, string type)
    {
        return principal.Claims.First(x => x.Type == type).Value;
    }
}
