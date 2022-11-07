using Ardalis.GuardClauses;
using System.Runtime.InteropServices;
using Vesta.Ddd.Domain.Auditing;

namespace Vesta.Banks.Traceability
{
    public class Error : CreationAuditedAggregateRoot<Guid>
    {
        public const string TableName = "Errors";
        public const int TypeMaxLength = 150;
        public const int MessageMaxLength  = 1000;
        public const int StackTraceMaxLength = 10000;

        public string Type { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public static Error Create(Exception exception)
        {
            Guard.Against.Null(exception);

            return new Error()
            {
                Type = exception.GetType().FullName,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            };
        }

    }
}
