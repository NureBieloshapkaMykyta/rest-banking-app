namespace Bank.Server.Shared.Constants;

public class ErrorMessages
{
    public class Auth
    {
        public const string InvadidEmailOrPassword = "Invalid email or password";
    }

    public class Account 
    {
        public const string NotFound = "Account not found";
        public const string NotEnoughBalance = "You have not enough money to perform operation";
    }

    public class Transaction
    {
        public const string InvalidType = "Invalid transaction type";
        public const string SameAccounts = "Cannot perform transaction between same accounts";
    }
}
