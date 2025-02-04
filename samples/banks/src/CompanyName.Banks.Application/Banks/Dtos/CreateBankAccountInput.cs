using System.ComponentModel.DataAnnotations;

namespace CompanyName.Banks.Dtos
{
    public class CreateBankAccountInput
    {
        [Required]
        public decimal Balance { get; set; }
    }
}