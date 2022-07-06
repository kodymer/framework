using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vesta.TestBase.Fixtures;
using Xunit;

namespace Vesta.Data.Fixtures
{
    [CollectionDefinition(nameof(DataContextCollection))]
    public class DataContextCollection :
        ICollectionFixture<InMemoryDbContextFixture>,
        ICollectionFixture<ServiceRegistrarFixture>
    {
    }
}
