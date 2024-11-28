namespace Bank.Server.Shared.Requests.Auth;

public record RegisterUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);
