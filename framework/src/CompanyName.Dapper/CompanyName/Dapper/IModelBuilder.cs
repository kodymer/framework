using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompanyName.Dapper.Metadata;

namespace CompanyName.Dapper
{
    public interface IModelBuilder
    {
        internal Dictionary<Type, object> EntityTypeBuilders { get; }

        DapperModelBuilder Entity<TEntity>(Action<DapperEntityTypeBuilder<TEntity>> buildAction) where TEntity : class;
    }
}
