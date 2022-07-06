using Vesta.Ddd.Application.Dtos;

namespace Vesta.Banks.Dtos
{
    public class BankTransferOutput
    {
        public string BankAccountFromNumber { get;  set; }
        public string BankAccountToNumber { get; set; }
        public decimal Amount { get; private set; }
        public DateTime CreationTime { get;  set; }
    }
}