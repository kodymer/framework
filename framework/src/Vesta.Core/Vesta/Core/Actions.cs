using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Core
{
    public static class Actions
    {
        public static void Empty() { }
        public static void Empty<T>(T t1) { }

        public static void Empty<T, K>(T t1, K t2) { }
    }
}
