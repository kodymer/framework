namespace CompanyName.Dapper
{
    public class DatabaseOptionsBuilder
    {
        public DatabaseOptions Options { get; private set; }

        public DatabaseOptionsBuilder(DatabaseOptions options)
        {
            Options = options;
        }

    }
}
