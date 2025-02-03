using System.ComponentModel.DataAnnotations;

namespace Vesta.Banks.Dtos
{
    public class CreateBankAccountInput
    {
        [Required]
        public decimal Balance { get; set; }
    }
}