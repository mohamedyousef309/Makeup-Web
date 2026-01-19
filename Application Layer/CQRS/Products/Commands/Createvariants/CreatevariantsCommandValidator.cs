using Domain_Layer.DTOs.ProductVariantDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.Createvariants
{
    public class CreatevariantsCommandValidator : AbstractValidator<CreatevariantsCommand>
    {
        public CreatevariantsCommandValidator()
        {
           

            RuleFor(x => x.UpdateProductVariantDtos)
                .NotNull()
                .WithMessage("Variants list cannot be null.")
                .NotEmpty()
                .WithMessage("At least one variant is required.");

            // validate each variant
            RuleForEach(x => x.UpdateProductVariantDtos)
                .SetValidator(new CreateProductVariantDtoValidator());
        }
    }

    public class CreateProductVariantDtoValidator
        : AbstractValidator<CreateProductVariantDto>
    {
        public CreateProductVariantDtoValidator()
        {
           

            RuleFor(x => x.VariantName)
                .NotEmpty()
                .WithMessage("Variant name is required.")
                .MaximumLength(100)
                .WithMessage("Variant name must not exceed 100 characters.");

            RuleFor(x => x.VariantValue)
                .NotEmpty()
                .WithMessage("Variant value is required.")
                .MaximumLength(100)
                .WithMessage("Variant value must not exceed 100 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Variant price cannot be negative.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Variant stock cannot be negative.");
        }
    }
}
