namespace Vesta.Banks.Bank
{
    public interface IBankTransferService
    {
        void MakeTransfer(BankAccount accountFrom, BankAccount accountTo, decimal amount);
    }
}