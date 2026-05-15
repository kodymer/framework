using AutoMapper;
using Samples.Banks.Accounts;

namespace Samples.Banks.AccountManagement
{
    public class AccountManagementProfile : Profile
    {
        public AccountManagementProfile()
        {
            CreateMap<BankAccount, BankAccountDto>(MemberList.None)
                .ForMember(d => d.Id, m => m.MapFrom(e => e.Id.Value));
        }
    }
}
