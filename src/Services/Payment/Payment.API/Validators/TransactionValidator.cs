using FluentValidation;
using PaymentService.Models;
using System;

namespace PaymentService.Validators
{
    public class TransactionValidator : AbstractValidator<TransactionDTO>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.Id).Must(id => id != Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Amount).Must(p => p > 0).WithMessage("Amount must be higher than 0.");
        }
    }
}
