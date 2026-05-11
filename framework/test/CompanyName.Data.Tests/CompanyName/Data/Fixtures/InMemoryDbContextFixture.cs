using CompanyName.Ddd.Domain.Entities;
using CompanyName.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace CompanyName.Data.Fixtures
{
    public class InMemoryDbContextFixture : IDisposable
    {
        private bool _disposed;

        public InMemoryCompanyNameDbContext DbContext { get; }

        public InMemoryDbContextFixture()
        {
            var options = new DbContextOptionsBuilder<InMemoryCompanyNameDbContext>()
                .UseInMemoryDatabase(databaseName: "Test")
                .Options;

            DbContext = new InMemoryCompanyNameDbContext(options);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources  
                    DbContext?.Dispose();
                }

                // Dispose unmanaged resources if any  

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public abstract class InMemoryCompanyNameDbContextBase<TContext> : CompanyNameDbContextBase<TContext>
        where TContext : DbContext
    {
        protected InMemoryCompanyNameDbContextBase(DbContextOptions<TContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<CompanyNameAggregateRoot>();
            modelBuilder.Entity<CompanyNameNonAggregateRoot>();
        }
    }

    public class InMemoryCompanyNameDbContext : InMemoryCompanyNameDbContextBase<InMemoryCompanyNameDbContext>
    {
        public InMemoryCompanyNameDbContext(DbContextOptions<InMemoryCompanyNameDbContext> options)
            : base(options)
        {
        }
    }


    public class TwoInMemoryCompanyNameDbContext : InMemoryCompanyNameDbContextBase<TwoInMemoryCompanyNameDbContext>
    {
        public TwoInMemoryCompanyNameDbContext(DbContextOptions<TwoInMemoryCompanyNameDbContext> options)
            : base(options)
        {
        }
    }

    public class CompanyNameAggregateRoot : AggregateRoot<int>
    {
        public CompanyNameAggregateRoot()
        {

        }

        public CompanyNameAggregateRoot(int id)
            : base(id)
        {

        }
    }

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
