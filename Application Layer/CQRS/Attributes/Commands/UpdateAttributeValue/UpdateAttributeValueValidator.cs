using FluentValidation;

namespace Application_Layer.CQRS.Attributes.Commands.UpdateAttributeValue
{
    public class UpdateAttributeValueValidator : AbstractValidator<UpdateAttributeValueCommand>
    {
        public UpdateAttributeValueValidator()
        {
            RuleFor(x => x.UpdateData.Id)
                .GreaterThan(0).WithMessage("Valid Value Id is required.");

            RuleFor(x => x.UpdateData.AttributeId)
                .GreaterThan(0).WithMessage("Valid Attribute Id is required.");

            RuleFor(x => x.UpdateData.Value)
                .NotEmpty().WithMessage("Value cannot be empty.")
                .MaximumLength(100).WithMessage("Value cannot exceed 100 characters.");
        }
    }
}