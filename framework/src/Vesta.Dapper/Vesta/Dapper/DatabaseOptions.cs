namespace Vesta.Dapper
{
    public class DatabaseOptions
    {
        private const int DefaultCommandTimeout = 3600;

        public string ConnectionString { get; set; }

        public int CommandTimeout { get; set; } = DefaultCommandTimeout;
    }
}