namespace Vesta.Banks.Etos
{
    public class BankAccountBalanceIncreasedEto
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public decimal AddedAmount { get; set; }

        public decimal Balance { get; set; }

    }
}
