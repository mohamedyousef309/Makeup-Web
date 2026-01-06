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
    }
}
