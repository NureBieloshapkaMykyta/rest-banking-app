using Bank.Server.Api.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Server.Api.Controllers;

[Authorize]
public class BaseBankController : Controller
{
    protected long UserId;

    public BaseBankController()
    {
        try
        {
            UserId = long.Parse(User.GetValueByClaimType(ClaimTypes.NameIdentifier));
        }
        catch
        {
            UserId = 0;
        }
    }
}
