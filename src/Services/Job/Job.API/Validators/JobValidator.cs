using FluentValidation;
using System;
using JobService.Models;

namespace JobService.Validators
{
    public class JobValidator : AbstractValidator<Job>
    {
        public JobValidator()
        {
            RuleFor(x => x.Id).Must(id => id != Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Name).NotEmpty().Length(5, 250);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
