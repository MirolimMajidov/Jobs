using FluentValidation;
using IdentityService.Models;
using System;

namespace IdentityService.Validators
{
    public class UserValidator : AbstractValidator<UserDTO>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id).Must(id => id != Guid.Empty).WithMessage("Id cannot be empty.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name must be have a value.");
            RuleFor(x => x.Login).NotEmpty().Length(3, 100);
        }
    }
}
