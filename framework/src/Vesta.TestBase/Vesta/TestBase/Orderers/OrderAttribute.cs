using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.TestBase.Orderers
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class OrderAttribute : Attribute
    {
        public OrderAttribute(int priority)
        {
            Priority = priority;
        }

        public int Priority { get; }
    }
}
