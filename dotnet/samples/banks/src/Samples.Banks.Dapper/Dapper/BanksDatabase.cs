using CompanyName.Dapper;

namespace Samples.Banks.Dapper
{
    public class BanksDatabase : CompanyNameDatabase<BanksDatabase>
    {
        public BanksDatabase()
        {

        }

        protected override void OnModelCreating(IModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureBanks();
        }
    }
}
