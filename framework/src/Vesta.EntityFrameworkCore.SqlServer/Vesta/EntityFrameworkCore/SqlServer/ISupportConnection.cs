namespace Vesta.EntityFrameworkCore.SqlServer
{
    public interface ISupportConnection
    {
        string ConnectionString { get; set; }
    }
}