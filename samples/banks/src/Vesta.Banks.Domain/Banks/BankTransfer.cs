using Vesta.Ddd.Domain.Auditing;

namespace Vesta.Banks
{
    public class BankTransfer : CreationAuditedEntity<long>
    {
        public const string TableName = "BankTransferHistory";

        public string BankAccountFromNumber { get; private set; }
        public string BankAccountToNumber { get; private set; }
        public decimal Amount { get; private set; }

        public BankTransfer(string bankAccountFromNumber, string bankAccountToNumber, decimal amount)
        {
            BankAccountFromNumber = bankAccountFromNumber;
            BankAccountToNumber = bankAccountToNumber;
            Amount = amount;

            CreationTime = DateTime.Now; // Adding value here because it not work with Entity Framework DbContext
        }

        public BankTransfer(string bankAccountFromNumber, string bankAccountToNumber, decimal amount, DateTime creationTime) // Necesary for Dapper
        {
            BankAccountFromNumber = bankAccountFromNumber;
            BankAccountToNumber = bankAccountToNumber;
            Amount = amount;

            CreationTime = creationTime;
        }
    }
}
