using Bank.Server.Business.Abstractions;
using Bank.Server.Shared.Requests.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionController(ITransactionService transactionService) : BaseBankController
{
    [HttpPost]
    public async Task<IActionResult> Perform([FromBody] PerformTransactionRequest request, CancellationToken cancellationToken = default)
    {
        var result = await transactionService.PerformTransactionAsync(UserId, request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }
}
