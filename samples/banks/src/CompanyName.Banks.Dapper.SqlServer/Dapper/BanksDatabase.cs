using CompanyName.Dapper;

namespace CompanyName.Banks.Dapper
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
