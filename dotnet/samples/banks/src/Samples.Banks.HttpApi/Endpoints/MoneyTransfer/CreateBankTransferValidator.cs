using FluentValidation;
using Samples.Banks.MoneyTransfers;

namespace Samples.Banks.Endpoints.MoneyTransfer
{
    internal sealed class CreateBankTransferValidator : AbstractValidator<CreateBankTransferCommand>
    {
        public CreateBankTransferValidator()
        {
            RuleFor(p => p.BankAccountFromId)
                .NotEmpty()
                .WithMessage("The bank account identification 'to' is required!");

            RuleFor(p => p.BankAccountFromId)
                .NotEmpty()
                .WithMessage("The bank account identification 'from' is required!");

            RuleFor(p => p.Amount)
                .LessThanOrEqualTo(decimal.Zero)
                .WithMessage("The transfer amount should be greater than 0.");
        }
    }
}