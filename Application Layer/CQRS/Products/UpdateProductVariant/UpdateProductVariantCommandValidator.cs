using Application_Layer.CQRS.Products.Commands.UpdateVariants;
using FluentValidation;

namespace Application_Layer.CQRS.Products.Validators
{
    public class UpdateProductVariantCommandValidator
        : AbstractValidator<UpdateProductVariantCommand>
    {
        public UpdateProductVariantCommandValidator()
        {
            RuleFor(x => x.UpdateProductVariantDtos)
                .NotNull()
                .WithMessage("Variants data is required.")
                .NotEmpty()
                .WithMessage("At least one variant is required.");

            RuleForEach(x => x.UpdateProductVariantDtos)
                .ChildRules(variant =>
                {
                    variant.RuleFor(v => v.Id)
                        .GreaterThan(0)
                        .WithMessage("Variant Id is required.");

                    variant.RuleFor(v => v.VariantName)
                        .NotEmpty()
                        .WithMessage("Variant name is required.");

                    variant.RuleFor(v => v.VariantValue)
                        .NotEmpty()
                        .WithMessage("Variant value is required.");

                    variant.RuleFor(v => v.Stock)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("Stock cannot be negative.");
                });
        }
    }
}
