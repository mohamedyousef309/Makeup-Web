using FluentValidation;

namespace Application_Layer.CQRS.Attributes.Commands.updateAttribute
{
    public class updateAttributeCommandValidator : AbstractValidator<updateAttributeCommand>
    {
        public updateAttributeCommandValidator()
        {
            RuleFor(x => x.UpdateData.Id)
                .NotEmpty().WithMessage("Id is required");

            RuleFor(x => x.UpdateData.AttributeName)
                .NotEmpty().WithMessage("Attribute name is required")
                .MinimumLength(2).WithMessage("Name must be at least 2 characters")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

            RuleForEach(x => x.UpdateData.Values)
                .NotEmpty().WithMessage("Value cannot be empty")
                .MaximumLength(100).WithMessage("Value cannot exceed 100 characters");
        }
    }
}