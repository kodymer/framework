using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Banks.Bank;

namespace Vesta.Banks.Application
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {

            CreateMap<BankAccount, BankAccountDto>();
        }
    }
}
