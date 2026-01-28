using Domain_Layer.DTOs.Attribute;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Attributes.Commands.AddAttributeValues
{
    public class AddAttributeValueCommandValidator
     : AbstractValidator<AddAttributeValueCommand>
    {
        public AddAttributeValueCommandValidator()
        {
            RuleFor(x => x.AddAttributeId)
              .GreaterThan(0)
              .WithMessage("AttributeId must be greater than 0");

            RuleFor(x => x.Values)
                .NotNull()
                .WithMessage("Values collection is required")
                .NotEmpty()
                .WithMessage("At least one value must be provided");

            RuleForEach(x => x.Values)
                .NotEmpty()
                .WithMessage("Attribute value cannot be empty")
                .MaximumLength(100)
                .WithMessage("Attribute value cannot exceed 100 characters");

            RuleFor(x => x.Values)
                .Must(values => values.Distinct(StringComparer.OrdinalIgnoreCase).Count() == values.Count())
                .WithMessage("Duplicate attribute values are not allowed");
        }
    }


   

}
