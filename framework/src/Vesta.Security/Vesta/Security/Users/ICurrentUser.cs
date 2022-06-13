using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Security.Users
{
    public interface ICurrentUser
    {
        public Guid? Id { get; }
        
        public string Name { get; }

        public string Email { get; }

    }
}
