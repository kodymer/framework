namespace Vesta.Banks.Etos
{
    public class BankAccountBalanceDecreasedEto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public decimal SubtractedAmount { get; set; }

        public decimal Balance { get; set; }

    }
}
