using CommunityToolkit.Diagnostics;
using CompanyName.Ddd.Domain.Auditing;

namespace Samples.Banks.Traceability
{
    public class ErrorRecord : CreationAuditedAggregateRoot<ErrorRecordId>
    {
        public const string TableName = "Errors";
        public const int TypeMaxLength = 150;
        public const int MessageMaxLength = 1000;
        public const int StackTraceMaxLength = 10000;

        public string Type { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public static ErrorRecord Create(Exception exception)
        {
            Guard.IsNotNull(exception);

            return new ErrorRecord()
            {
                Type = exception.GetType().FullName,
                Message = exception.Message,
                StackTrace = exception.StackTrace
            };
        }

    }
}
