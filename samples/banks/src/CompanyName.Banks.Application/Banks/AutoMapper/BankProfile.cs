using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Banks.Bank;
using Vesta.Banks.Dtos;

namespace Vesta.Banks.AutoMapper
{
    public class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
            CreateMap<BankTransfer, BankTransferOutput>();
        }
    }
}
