using Domain_Layer.DTOs.ProductVariantDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands
{
    
    
        public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
        {
            public CreateProductCommandValidator()
            {
                // Validate product main fields
                RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Product name is required.")
                    .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

                RuleFor(x => x.Description)
                    .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

                RuleFor(x => x.Price)
                    .GreaterThan(0).WithMessage("Price must be greater than zero.");

                RuleFor(x => x.Stock)
                    .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

                RuleFor(x => x.CategoryId)
                    .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

                // Validate variants if any
                When(x => x.Variants != null && x.Variants.Any(), () =>
                {
                    RuleForEach(x => x.Variants).SetValidator(new ProductVariantValidator());
                });
            }
        }
    public class ProductVariantValidator : AbstractValidator<CreateProductVariantDto>
    {
        public ProductVariantValidator()
        {
            RuleFor(v => v.VariantName)
                .NotEmpty().WithMessage("Variant name is required.")
                .MaximumLength(100).WithMessage("Variant name must not exceed 100 characters.");

            RuleFor(v => v.VariantValue)
                .NotEmpty().WithMessage("Variant value is required.")
                .MaximumLength(100).WithMessage("Variant value must not exceed 100 characters.");

            RuleFor(v => v.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Variant stock cannot be negative.");
        }
    }
}

