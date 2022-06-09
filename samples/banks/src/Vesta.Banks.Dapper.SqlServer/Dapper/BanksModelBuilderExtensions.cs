using Vesta.Dapper;

namespace Vesta.Banks.Dapper
{
    public static class BanksModelBuilderExtensions
    {
        public static void ConfigureBanks(this IModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankAccount>(b =>
            {
                b.ToTable(BankAccount.TableName);

            }).Entity<BankTransfer>(b =>
            {
                b.ToTable(BankTransfer.TableName);
            });
        }
    }
}
