using FluentValidation;

namespace Application_Layer.CQRS.Products.Commands.AddVariantsToProduct
{
    public class AddVariantsToProductValidator : AbstractValidator<AddVariantsToProductCommand>
    {
        public AddVariantsToProductValidator() 
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.")
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.");

            RuleFor(x => x.VariantIds)
                .NotEmpty().WithMessage("Variant IDs list cannot be empty.")
                .Must(ids => ids != null && ids.Count > 0)
                .WithMessage("At least one Variant ID must be provided.");

            RuleFor(x => x.VariantIds)
                .Must(ids => ids != null && ids.Distinct().Count() == ids.Count)
                .WithMessage("Duplicate Variant IDs are not allowed.");
        }
    }
}