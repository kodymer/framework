using FluentValidation;
using Samples.Banks.AccountManagement;

namespace Samples.Banks.Endpoints.AccountManagement
{
    internal sealed class CreateBankAccountCommandValidator : AbstractValidator<CreateBankAccountCommand>
    {
        public CreateBankAccountCommandValidator()
        {
            RuleFor(p => p.Balance)
                .GreaterThan(decimal.Zero)
                .WithMessage("The balance can not be negative or zero!");
        }
    }
}