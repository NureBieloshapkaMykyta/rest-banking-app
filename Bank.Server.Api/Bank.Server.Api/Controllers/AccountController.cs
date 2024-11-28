using Bank.Server.Api.Extensions;
using Bank.Server.Business.Abstractions;
using Bank.Server.Shared.Requests.Account;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Bank.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(IAccountService accountService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var userId = long.Parse(User.GetValueByClaimType(ClaimTypes.NameIdentifier));
        var result = await accountService.CreateAccountAsync(userId, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var userId = long.Parse(User.GetValueByClaimType(ClaimTypes.NameIdentifier));
        var result = await accountService.GetUserAccountsAsync(userId, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }

    [HttpGet("{number:guid}")]
    public async Task<IActionResult> GetByNumber([FromRoute] Guid number, CancellationToken cancellationToken = default)
    {
        var userId = long.Parse(User.GetValueByClaimType(ClaimTypes.NameIdentifier));
        var result = await accountService.GetAccountDetailsByNumberAsync(new GetAccountDetailsRequest(userId, number), cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}
