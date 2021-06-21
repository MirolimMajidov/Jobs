using FluentValidation;
using PaymentService.Models;
using System;

namespace PaymentService.Validators
{
    public class PaymentValidator : AbstractValidator<Payment>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.Id).Must(id => id != Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Amount).Must(p => p > 0).WithMessage("Amount must be higher than 0.");
            RuleFor(x => x.Sender).NotEmpty();
        }
    }
}
