using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Data.Fixtures;
using Xunit;

namespace CompanyName.Uow.Fixtures
{
    [CollectionDefinition(nameof(UnitOfWorkCollectionFixture))]
    public class UnitOfWorkCollectionFixture : 
        ICollectionFixture<InMemoryDbContextFixture>,
        ICollectionFixture<UnitOfWorkServiceRegistrarFixture>
    {
    }
}
