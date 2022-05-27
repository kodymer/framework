using System;
using System.Runtime.Serialization;

namespace Vesta.ProjectName.Bank
{
    [Serializable]
    internal class InsufficientBalanceException : Exception
    {
        public InsufficientBalanceException()
        {
        }

        public InsufficientBalanceException(string message) : base(message)
        {
        }

        public InsufficientBalanceException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InsufficientBalanceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}