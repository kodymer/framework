using CompanyName.Ddd.Domain.Entities;

namespace CompanyName.Data.Fixtures
{
    public class CompanyNameNonAggregateRoot : Entity<int>
    {
        public CompanyNameNonAggregateRoot()
        {

        }

        public CompanyNameNonAggregateRoot(int id)
            : base(id)
        {

        }
    }
}
