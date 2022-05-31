using System.ComponentModel.DataAnnotations;

namespace Vesta.Banks.Bank
{
    public class CreateBankAccountInput
    {
        [Required]
        public decimal Balance { get; set; }
    }
}