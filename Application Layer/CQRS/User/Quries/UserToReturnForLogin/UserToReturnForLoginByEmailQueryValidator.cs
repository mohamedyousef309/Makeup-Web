using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Quries.UserToReturnForLogin
{
    public class UserToReturnForLoginByEmailQueryValidator : AbstractValidator<UserToReturnForLoginByEmailQuery>
    {
        public UserToReturnForLoginByEmailQueryValidator()
        {
            RuleFor(x => x.UserEmail)
               .NotEmpty()
               .WithMessage("Email is required")

               .EmailAddress()
               .WithMessage("Invalid email format");
        }
    }
}
