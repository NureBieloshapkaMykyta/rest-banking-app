using Bank.Server.Business.Abstractions;
using Bank.Server.Shared.Requests.Account;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(IAccountService accountService) : BaseBankController
{
    [HttpPost]
    public async Task<IActionResult> Create(CancellationToken cancellationToken = default)
    {
        var result = await accountService.CreateAccountAsync(UserId, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        var result = await accountService.GetUserAccountsAsync(UserId, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }

    [HttpGet("{number:guid}")]
    public async Task<IActionResult> GetByNumber([FromRoute] Guid number, CancellationToken cancellationToken = default)
    {
        var result = await accountService.GetAccountDetailsByNumberAsync(new GetAccountDetailsRequest(UserId, number), cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}
