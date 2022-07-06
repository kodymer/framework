using Vesta.Dapper;

namespace Vesta.Banks.Dapper
{
    public class BanksDatabase : VestaDatabase<BanksDatabase>
    {
        public BanksDatabase()
            : base()
        {

        }

        protected override void OnModelCreating(IModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureBanks();
        }
    }
}
