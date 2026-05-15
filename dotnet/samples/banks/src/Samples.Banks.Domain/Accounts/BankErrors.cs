using FluentResults;

namespace Samples.Banks.Accounts
{

    public static class BankErrors
    {
        public static Result InsufficientBalance => new InsufficientBalanceError("Insufficient balance to complete the transaction");

        public static Result UnfulfilledRequirement => new Error("The account does not meet the minimum requirements to be opened.");

        public static Result CouldNotDisactivate => new Error("The bank account cannot be deactivated because it has a positive balance.");

        public static Result CouldNotActivate => new Error("The bank account cannot be activated because it has been inactive for more than 6 months.");
    }


    public class InsufficientBalanceError : Error
    {
        public InsufficientBalanceError(string message)
            : base(message)
        {

            WithMetadata("ErrorCode", "BankErrors.InsufficientBalance");
        }
    }

    public class UnfulfilledRequirementError : Error
    {
        public UnfulfilledRequirementError(string message, IError causedBy)
            : base(message)
        {
            Reasons.Add(causedBy);

            WithMetadata("ErrorCode", "BankErrors.UnfulfilledRequirement");
        }
    }
}
