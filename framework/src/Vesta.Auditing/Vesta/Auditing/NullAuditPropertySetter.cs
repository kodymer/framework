using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Auditing
{
    public class NullAuditPropertySetter : IAuditPropertySetter
    {
        private static IAuditPropertySetter _instance;
        public static IAuditPropertySetter Instance
        {
            get
            {
                _instance ??= new NullAuditPropertySetter();
                return _instance;
            }
        }

        public void SetCreationProperties(object targetObject)
        {

        }

        public void SetDeletionProperties(object targetObject)
        {

        }

        public void SetModificationProperties(object targetObject)
        {

        }
    }
}
