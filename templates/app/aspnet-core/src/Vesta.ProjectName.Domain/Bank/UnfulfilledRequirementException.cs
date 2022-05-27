using System;
using System.Runtime.Serialization;
using Vesta.ProjectName.Domain.Bank;

namespace Vesta.ProjectName.Bank
{
    [Serializable]
    public class UnfulfilledRequirementException : BusinessException
    {
        public UnfulfilledRequirementException()
        {
        }

        public UnfulfilledRequirementException(string message) : base(message)
        {
        }

        public UnfulfilledRequirementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}