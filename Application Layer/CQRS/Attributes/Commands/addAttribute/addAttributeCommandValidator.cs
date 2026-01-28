using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Attributes.Commands.addAttribute
{
    public class addAttributeCommandValidator:AbstractValidator<addAttributeCommand>
    {
        public addAttributeCommandValidator()
        {
            RuleFor(x => x.AttributeName)
           .NotEmpty().WithMessage("Attribute name is required")
           .MinimumLength(2).WithMessage("Attribute name must be at least 2 characters")
           .MaximumLength(50).WithMessage("Attribute name must not exceed 50 characters");

            RuleForEach(x => x.Values)
                .NotNull().WithMessage("Attribute value cannot be null")
                .NotEmpty().WithMessage("Attribute value cannot be empty")
                .MaximumLength(100).WithMessage("Each attribute value must not exceed 100 characters")
                .When(x => x.Values != null && x.Values.Any());
        }
    }
}
