using Domain_Layer.DTOs.ProductVariantDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.Createvariants
{
    public class CreatevariantsCommandValidator:AbstractValidator<CreatevariantsCommand>
    {
        public CreatevariantsCommandValidator()
        {
            RuleFor(x => x.UpdateProductVariantDtos)
                .NotNull()
                .WithMessage("Variants list cannot be null.")
                .NotEmpty()
                .WithMessage("At least one variant is required.");

            // Validate each item in the collection
            RuleForEach(x => x.UpdateProductVariantDtos)
                .SetValidator(new UpdateProductVariantDtoValidator());
        }
    }


    public class UpdateProductVariantDtoValidator
      : AbstractValidator<UpdateProductVariantDto>
    {
        public UpdateProductVariantDtoValidator()
        {
          

            RuleFor(x => x.VariantName)
                .NotEmpty()
                .WithMessage("Variant name is required.")
                .MaximumLength(100);

            RuleFor(x => x.VariantValue)
                .NotEmpty()
                .WithMessage("Variant value is required.")
                .MaximumLength(100);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock cannot be negative.");
        }
    }
}
