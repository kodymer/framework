using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyName.Security.Users
{
    public interface ICurrentUser
    {

        public object Id { get; }

        public string Name { get; }

        public string Role { get; }

        public string Email { get; }

        T GetId<T>() 
            where T : struct, IParsable<T>;

        bool TryGetId<T>(out T id)
            where T : struct, IParsable<T>;
    }
}
