using Application_Layer.CQRS.Authantication.Commads.ValidateUserVerificationCode;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.ValidateUserToken
{
    public class ValidatVerificationCodeCommandValidator: AbstractValidator<ValidateVerificationCodeOrchestrator>
    {
        public ValidatVerificationCodeCommandValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage("Request cannot be null.");

            RuleFor(x => x.token)
                .NotEmpty()
                .WithMessage("Verification code is required.")
                .Length(6)
                .WithMessage("Verification code must be exactly 6 digits.");

            RuleFor(x => x.userEmail)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Please enter a valid email address.");
        }
    }
}
