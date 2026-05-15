using CompanyName.Ddd.Application.Dtos;

namespace Samples.Banks.AccountManagement
{
    public record class BankAccountDto : EntityDto<Guid>
    {
        public string Number { get; set; }

        public decimal Balance { get; set; }
    }
}