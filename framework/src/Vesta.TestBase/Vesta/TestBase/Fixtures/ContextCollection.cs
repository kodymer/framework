using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Vesta.TestBase.Fixtures
{
    [CollectionDefinition(nameof(ContextCollection))]
    public class ContextCollection :
        ICollectionFixture<InMemoryDbContextFixture>,
        ICollectionFixture<ServiceRegistrarFixture>
    {
    }
}
