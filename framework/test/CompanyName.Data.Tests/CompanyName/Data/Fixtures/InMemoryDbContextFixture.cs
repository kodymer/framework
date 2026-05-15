using FluentAssertions.Primitives;
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
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
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
}
