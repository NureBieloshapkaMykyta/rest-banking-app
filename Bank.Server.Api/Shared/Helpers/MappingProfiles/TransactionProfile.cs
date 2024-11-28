using AutoMapper;
using Bank.Server.Core.Entities;
using Bank.Server.Shared.Responses.Account;
using Bank.Server.Shared.Responses.Transactions;

namespace Bank.Server.Shared.Helpers.MappingProfiles;

public class TransactionProfile : Profile
{
    public TransactionProfile()
    {
        CreateMap<Transaction, TransactionResponse>();
    }
}