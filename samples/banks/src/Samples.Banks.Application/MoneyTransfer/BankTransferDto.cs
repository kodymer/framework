using CompanyName.Ddd.Application.Dtos;


namespace Samples.Banks.MoneyTransfers
{
    public record class BankTransferDto
    {
        public string BankAccountFromNumber { get; set; }

        public string BankAccountToNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreationTime { get; set; }
    }
}