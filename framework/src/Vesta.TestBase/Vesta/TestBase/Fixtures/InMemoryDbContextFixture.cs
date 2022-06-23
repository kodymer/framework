using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using Vesta.Ddd.Domain.Entities;
using Vesta.EntityFrameworkCore;
using Vesta.EntityFrameworkCore.Abstracts;

namespace Vesta.TestBase.Fixtures
{
    public class InMemoryDbContextFixture : IDisposable
    {
        public InMemoryVestaDbContextProvider DbContextProvider { get; }

        public InMemoryVestaDbContext DbContext { get; }

        public InMemoryDbContextFixture()
        {
            DbContextProvider = new InMemoryVestaDbContextProvider();
            DbContext = AsyncContext.Run(async () => await DbContextProvider.GetDbContextAsync());
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

    }

    public class InMemoryVestaDbContextProvider : IDbContextProvider<InMemoryVestaDbContext>
    {
        public Task<InMemoryVestaDbContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<InMemoryVestaDbContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;

            return Task.FromResult(new InMemoryVestaDbContext(options));
        }
    }

    public class InMemoryVestaDbContext : VestaDbContextBase<InMemoryVestaDbContext>
    {
        public InMemoryVestaDbContext(DbContextOptions<InMemoryVestaDbContext> options)
            : base(options)
        {
        }

        protected override string GetConnectionString(DbContextOptions<InMemoryVestaDbContext> options)
        {
            return string.Empty;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<VestaEntity>();
        }
    }

    public class VestaEntity : Entity<int>
    {
        public VestaEntity()
        {

        }

        public VestaEntity(int id)
            : base(id)
        {

        }
    }
}
