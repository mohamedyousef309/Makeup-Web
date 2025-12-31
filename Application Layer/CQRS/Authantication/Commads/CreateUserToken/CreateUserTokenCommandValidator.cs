using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.CreateUserToken
{
    public class CreateUserTokenCommandValidator : AbstractValidator<CreateUserTokenCommand>
    {
        public CreateUserTokenCommandValidator()
        {
            RuleFor(x => x)
           .NotNull()
           .WithMessage("Command cannot be null.");

            RuleFor(x => x.userid)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.token)
                .NotNull()
                .WithMessage("Token cannot be null.")
                .NotEmpty()
                .WithMessage("Token is required.");

        }
    }
}
