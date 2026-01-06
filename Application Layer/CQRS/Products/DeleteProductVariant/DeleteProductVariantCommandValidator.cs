using Application_Layer.CQRS.Products.Commands.UpdateVariants;
using FluentValidation;

namespace Application_Layer.CQRS.Products.Validators
{
    public class DeleteProductVariantCommandValidator
        : AbstractValidator<DeleteProductVariantCommand>
    {
        public DeleteProductVariantCommandValidator()
        {
            RuleFor(x => x.VariantId)
                .GreaterThan(0)
                .WithMessage("Variant Id is required and must be greater than zero.");
        }
    }
}
