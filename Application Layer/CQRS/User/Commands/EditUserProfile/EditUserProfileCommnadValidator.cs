using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Commands.EditUserProfile
{
    public class EditUserProfileCommnadValidator:AbstractValidator<EditUserProfileCommnad>
    {
        public EditUserProfileCommnadValidator()
        {
            RuleFor(x => x.userid)
            .NotEmpty().WithMessage("User ID is required.")
            .GreaterThan(0).WithMessage("Invalid User ID.");

            RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Please enter a valid email address.");

            RuleFor(x => x.UserAddress)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^01[0125][0-9]{8}$").WithMessage("Please enter a valid Egyptian phone number.");
            // النمط أعلاه مخصص لأرقام الهواتف المصرية، يمكنك تعديله حسب دولتك
        }
    }
}
