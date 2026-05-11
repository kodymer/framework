using CompanyName.Data.Fixtures;
using CompanyName.TestBase.Fixtures;
using Xunit;

namespace CompanyName.Data.Fixtures
{
    [CollectionDefinition(nameof(DataContextCollection))]
    public class DataContextCollection :
        ICollectionFixture<InMemoryDbContextFixture>,
        ICollectionFixture<ServiceRegistrarFixture>
    {

        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
