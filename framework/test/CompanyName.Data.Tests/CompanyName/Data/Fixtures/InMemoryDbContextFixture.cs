using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using System;
using System.Threading;
using System.Threading.Tasks;
using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore;
using CompanyName.EntityFrameworkCore.Abstracts;

namespace CompanyName.Data.Fixtures
{
    public class InMemoryDbContextFixture : IDisposable
    {
        public InMemoryCompanyNameDbContextProvider DbContextProvider { get; }

        public InMemoryCompanyNameDbContext DbContext { get; }

        public InMemoryDbContextFixture()
        {
            DbContextProvider = new InMemoryCompanyNameDbContextProvider();
            DbContext = AsyncContext.Run(async () => await DbContextProvider.GetDbContextAsync());
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }

    }

    public class InMemoryCompanyNameDbContextProvider : IDbContextProvider<InMemoryCompanyNameDbContext>
    {
        public Task<InMemoryCompanyNameDbContext> GetDbContextAsync(CancellationToken cancellationToken = default)
        {
            var options = new DbContextOptionsBuilder<InMemoryCompanyNameDbContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;

            return Task.FromResult(new InMemoryCompanyNameDbContext(options));
        }
    }

    public class InMemoryCompanyNameDbContext : CompanyNameDbContextBase<InMemoryCompanyNameDbContext>
    {
        public InMemoryCompanyNameDbContext(DbContextOptions<InMemoryCompanyNameDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CompanyNameEntity>();
        }
    }

    public class CompanyNameEntity : Entity<int>
    {
        public CompanyNameEntity()
        {

        }

        public CompanyNameEntity(int id)
            : base(id)
        {

        }
    }
}
