namespace CompanyName.Uow
{
    public interface ITransactionApiContainer
    {
        void AddTransactionApi(string key, ITransactionApi api);

        ITransactionApi FindTransactionApi(string key);

    }
}