using Application_Layer.CQRS.Products.Commands.UpdateVariants;
using FluentValidation;

namespace Application_Layer.CQRS.Products.Validators
{
    public class UpdateProductVariantCommandValidator
        : AbstractValidator<UpdateProductVariantCommand>
    {
        public UpdateProductVariantCommandValidator()
        {
            RuleFor(x => x.Id)
           .GreaterThan(0)
           .WithMessage("Variant Id must be greater than 0");

            RuleFor(x => x.VariantName)
                .NotEmpty().WithMessage("Variant name is required")
                .MinimumLength(2).WithMessage("Variant name must be at least 2 characters")
                .MaximumLength(100).WithMessage("Variant name must not exceed 100 characters");

            RuleFor(x => x.VariantValue)
                .NotEmpty().WithMessage("Variant value is required")
                .MaximumLength(100).WithMessage("Variant value must not exceed 100 characters");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock cannot be negative");
        
        }
    }
}
