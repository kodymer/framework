using CompanyName.Dapper;
using Samples.Banks.Accounts;
using Samples.Banks.Transfers;

namespace Samples.Banks.Dapper
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
