using AutoMapper;
using Samples.Banks.AccountManagement;
using Samples.Banks.Transfers;

namespace Samples.Banks.MoneyTransfers
{
    public class MoneyTransferProfile : Profile
    {
        public MoneyTransferProfile()
        {
            CreateMap<BankTransfer, BankTransferDto>();
        }
    }
}
