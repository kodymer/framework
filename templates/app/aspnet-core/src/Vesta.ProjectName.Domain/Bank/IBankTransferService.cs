namespace Vesta.ProjectName.Bank
{
    public interface IBankTransferService
    {
        void MakeTransfer(BankAccount accountFrom, BankAccount accountTo, decimal amount);
    }
}