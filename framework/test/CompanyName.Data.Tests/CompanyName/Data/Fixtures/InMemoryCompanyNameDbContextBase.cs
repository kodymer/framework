using CompanyName.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.Data.Fixtures
{
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
}
