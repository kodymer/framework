namespace Vesta.Banks.Etos
{
    public class BankAccountChangedEto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public decimal Balance { get; set; }
    }
}
