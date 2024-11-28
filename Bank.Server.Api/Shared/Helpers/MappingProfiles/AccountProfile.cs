using AutoMapper;
using Bank.Server.Core.Entities;
using Bank.Server.Shared.Responses.Account;

namespace Bank.Server.Shared.Helpers.MappingProfiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<Account, AccountResponse>();

        CreateMap<Account, AccountDetailsResponse>();
    }
}
