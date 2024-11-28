using Bank.Server.Business.Abstractions;
using Bank.Server.Shared.Requests.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Bank.Server.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : Controller
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authService.RegisterUserAsync(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] SignInRequest request, CancellationToken cancellationToken = default)
    {
        var result = await authService.SignIn(request, cancellationToken);

        if (!result.IsSuccessful)
        {
            return BadRequest(result.Message);
        }

        return Ok(result.Data);
    }
}
