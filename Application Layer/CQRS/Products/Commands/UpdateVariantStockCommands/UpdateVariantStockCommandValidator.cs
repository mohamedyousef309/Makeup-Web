using Application_Layer.CQRS.Products.Commands.UpdateVariantStock;
using FluentValidation;

namespace Application_Layer.CQRS.Products.Validators
{
    public class UpdateVariantStockCommandValidator : AbstractValidator<UpdateVariantStockCommand>
    {
        public UpdateVariantStockCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Invalid Variant ID.");

            RuleFor(x => x.NewStock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
        }
    }
}