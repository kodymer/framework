namespace CompanyName.Uow
{
    public interface IDatabaseApiContainer
    {
        void AddDatabaseApi(string key, IDatabaseApi databaseApi);

        IDatabaseApi FindDatabaseApi(string key);
    }
}