using FluentValidation;

namespace Application_Layer.CQRS.Attributes.Commands.DeleteAttributeValue
{
    public class DeleteAttributeValueCommandValidator : AbstractValidator<DeleteAttributeValueCommand>
    {
        public DeleteAttributeValueCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Attribute Value Id is required.")
                .GreaterThan(0)
                .WithMessage("Attribute Value Id must be a positive number greater than 0.");
        }
    }
}