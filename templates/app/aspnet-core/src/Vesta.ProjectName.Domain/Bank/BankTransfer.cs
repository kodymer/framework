using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Ddd.Domain.Auditing;

namespace Vesta.ProjectName.Domain.Bank
{
    public class BankTransfer : CreationAuditedEntity<long>
    {
        public string BankAccountNumberFrom { get; set; }
        public string BankAccountNumberTo { get; set; }
        public decimal Amount { get; set; }
    }
}
