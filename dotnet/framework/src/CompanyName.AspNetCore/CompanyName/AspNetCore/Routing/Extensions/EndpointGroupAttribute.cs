using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.AspNetCore.Routing.Extensions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class EndpointGroupAttribute : Attribute
    {
        public string Name { get; }

        public EndpointGroupAttribute(string name)
        {
            Name = name;
        }
    }

}
