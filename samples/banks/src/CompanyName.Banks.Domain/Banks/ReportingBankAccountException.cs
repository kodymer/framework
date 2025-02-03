using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vesta.Banks
{
    public class ReportingBankAccountException : BusinessException
    {
        public ReportingBankAccountException()
        {
        }

        public ReportingBankAccountException(string message) : base(message)
        {
        }

        public ReportingBankAccountException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
