namespace Vesta.Uow
{
    public interface ITransacionApiContainer
    {
        void AddTransactionApi(string key, ITransactionApi api);

        ITransactionApi FindTransactionApi(string key);

    }
}