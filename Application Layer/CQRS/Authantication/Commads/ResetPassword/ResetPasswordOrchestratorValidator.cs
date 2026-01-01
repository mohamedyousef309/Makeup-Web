using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.ResetPassword
{
    public class ResetPasswordOrchestratorValidator:AbstractValidator<ResetPasswordOrchestrator>
    {
        public ResetPasswordOrchestratorValidator()
        {
            RuleFor(x => x.UserEmail)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters");
        }
    }
}
