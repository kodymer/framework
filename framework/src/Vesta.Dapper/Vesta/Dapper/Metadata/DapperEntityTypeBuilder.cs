using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Dapper.Metadata
{
    public class DapperEntityTypeBuilder<TEntity> : IEntityTypeBuilder
        where TEntity : class
    {
        Dictionary<Type, string> IEntityTypeBuilder.Tables => _tables;

        private Dictionary<Type, string> _tables;

        public DapperEntityTypeBuilder()
        {
            _tables = new Dictionary<Type, string>();
        }

        public DapperEntityTypeBuilder<TEntity> ToTable(string name)
        {
            if (!_tables.ContainsKey(typeof(TEntity)))
            {
                _tables.Add(typeof(TEntity), name ?? typeof(TEntity).Name);
            }
            else
            {
                _tables[typeof(TEntity)] = name ?? typeof(TEntity).Name;
            }

            return this;
        }
    }
}
