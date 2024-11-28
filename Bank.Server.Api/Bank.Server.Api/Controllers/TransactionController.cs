using Bank.Server.Business.Abstractions;
using Bank.Server.Shared.Requests.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class TransactionController(ITransactionService transactionService) : Controller
{
    [HttpPost]
    public async Task<IActionResult> Perform([FromBody] PerformTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var result = await transactionService.PerformTransactionAsync(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }
}
