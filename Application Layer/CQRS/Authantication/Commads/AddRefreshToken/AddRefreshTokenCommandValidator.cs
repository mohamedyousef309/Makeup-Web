using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.AddRefreshToken
{
    public class AddRefreshTokenCommandValidator:AbstractValidator<AddRefreshTokenCommand>
    {
        public AddRefreshTokenCommandValidator()
        {
            RuleFor(x => x.RefreshTokens.userid)
             .GreaterThan(0)
             .WithMessage("UserId must be a valid positive number.");

            RuleFor(x => x.RefreshTokens.Token)
                .NotEmpty()
                .WithMessage("Refresh token must not be empty.")
                .MinimumLength(32)
                .WithMessage("Refresh token must be at least 32 characters long.");

            RuleFor(x => x.RefreshTokens.CreatedAt)
                .NotEmpty()
                .WithMessage("CreatedAt is required.");

            RuleFor(x => x.RefreshTokens.ExpiresOn)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Refresh token must expire in the future.");

            RuleFor(x => x.RefreshTokens.RevokedOn)
                .GreaterThan(x => x.RefreshTokens.CreatedAt)
                .When(x => x.RefreshTokens.RevokedOn.HasValue)
                .WithMessage("RevokedOn must be after CreatedAt.");
        }

    }
}
