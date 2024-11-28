using System.ComponentModel.DataAnnotations;

namespace Bank.Server.Shared.Requests.Auth;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    [EmailAddress]string Email,
    [MinLength(6)]string Password);
