using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.ForgetPasswrd
{
    public class ForgetpasswordOrchestratorValidator:AbstractValidator<ForgetpasswordOrchestrator>
    {
        public ForgetpasswordOrchestratorValidator()
        {
            RuleFor(x => x)
          .NotNull()
          .WithMessage("Command cannot be null.");

            RuleFor(x => x.UserGmail)
                .NotNull()
                .WithMessage("Email cannot be null.")
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Invalid email format.");
        }
    }
}
