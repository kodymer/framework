using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.Data.Fixtures;
using Xunit;

namespace Vesta.Uow.Fixtures
{
    [CollectionDefinition(nameof(UnitOfWorkCollectionFixture))]
    internal class UnitOfWorkCollectionFixture : 
        ICollectionFixture<InMemoryDbContextFixture>,
        ICollectionFixture<UnitOfWorkServiceRegistrarFixture>
    {
    }
}
