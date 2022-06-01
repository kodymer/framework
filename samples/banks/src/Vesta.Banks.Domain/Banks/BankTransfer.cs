using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Ddd.Domain.Auditing;

namespace Vesta.Banks
{
    public class BankTransfer : CreationAuditedEntity<long>
    {
        public BankTransfer(Guid bankAccountFromId, Guid bankAccountToId, decimal amount)
        {
            Guard.Against.NegativeOrZero(amount, nameof(amount));

            BankAccountFromId = bankAccountFromId;
            BankAccountToId = bankAccountToId;
            Amount = amount;
        }

        public Guid BankAccountFromId { get; private set; }
        public Guid BankAccountToId { get; private set; }
        public decimal Amount { get; private set; }
    }
}
