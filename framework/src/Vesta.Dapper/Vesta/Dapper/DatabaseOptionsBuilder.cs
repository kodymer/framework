using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Dapper
{
    public class DatabaseOptionsBuilder
    {

        public DatabaseOptions Options { get; private set; }

        public DatabaseOptionsBuilder(DatabaseOptions options)
        {
            Options = options;
        }

    }
}
