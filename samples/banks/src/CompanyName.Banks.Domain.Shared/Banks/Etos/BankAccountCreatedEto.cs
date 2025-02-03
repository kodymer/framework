namespace Vesta.Banks.Etos
{
    public class BankAccountCreatedEto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public decimal InitialBalance { get; set; }
    }
}
