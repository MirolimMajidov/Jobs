using FluentValidation;
using JobService.Models;
using System;

namespace JobService.Validators
{
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator()
        {
            RuleFor(x => x.Id).Must(id => id != Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Name).NotEmpty().Length(5, 250);
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
