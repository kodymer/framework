using System;
using System.ComponentModel.DataAnnotations;

namespace CompanyName.Banks.Dtos
{

    public class BankTransferInput
    {
        [Required]
        public Guid BankAccountFromId { get; set; }

        [Required]
        public Guid BankAccountToId { get; set; }

        public decimal Amount { get; set; }
    }
}