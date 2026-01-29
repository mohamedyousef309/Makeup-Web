using FluentValidation;

namespace Application_Layer.CQRS.Attributes.Commands.deleteAttribute
{
    public class deleteAttributeCommandValidator : AbstractValidator<deleteAttributeCommand>
    {
        public deleteAttributeCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}