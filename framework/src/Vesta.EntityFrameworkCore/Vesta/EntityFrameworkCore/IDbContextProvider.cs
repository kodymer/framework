using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.EntityFrameworkCore
{
    public interface IDbContextProvider<TDbContext>
        where TDbContext : IEfCoreDbContext
    {
        Task<TDbContext> GetDbContextAsync();
    }
}
