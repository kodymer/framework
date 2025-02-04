using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Banks.Bank;
using CompanyName.Banks.Dtos;

namespace CompanyName.Banks.AutoMapper
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
