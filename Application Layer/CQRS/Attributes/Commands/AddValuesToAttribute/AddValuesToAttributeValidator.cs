using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Attributes.Commands.AddValuesToAttribute
{
    public class AddValuesToAttributeValidator : AbstractValidator<AddValuesToAttributeCommand>
    {
        public AddValuesToAttributeValidator()
        {
            RuleFor(x => x.AddData.AttributeId)
                .GreaterThan(0).WithMessage("Please provide a valid Attribute Id.");

            RuleFor(x => x.AddData.Values)
                .NotNull().WithMessage("Values list cannot be null.")
                .NotEmpty().WithMessage("You must provide at least one value to add.");

            RuleForEach(x => x.AddData.Values)
                .NotEmpty().WithMessage("Value text cannot be empty.")
                .MaximumLength(100).WithMessage("Value text is too long.");

           
            RuleFor(x => x.AddData.Values)
                .Must(v => v.Distinct(StringComparer.OrdinalIgnoreCase).Count() == v.Count())
                .WithMessage("Duplicate values in the same request are not allowed.");
        }
    }
}
