using System.ComponentModel.DataAnnotations;

namespace Vesta.ProjectName.Bank
{
    public class CreateBankAccountInput
    {
        [Required]
        public decimal Balance { get; set; }
    }
}