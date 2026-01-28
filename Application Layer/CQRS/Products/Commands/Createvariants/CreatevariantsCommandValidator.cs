using Domain_Layer.DTOs.Attribute;
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


            RuleFor(x => x.productid)
            .GreaterThan(0)
            .WithMessage("Invalid Product Id");

            RuleFor(x => x.UpdateProductVariantDtos)
                .NotNull()
                .NotEmpty()
                .WithMessage("At least one product variant is required");

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
                      .MaximumLength(50);

            RuleFor(x => x.VariantValue)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.AttributeValueId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Each variant must have attribute values");

            RuleFor(x => x.AttributeValueId.Count())
                .GreaterThan(0)
                .WithMessage("Variant must contain at least one attribute value");

            // منع تكرار نفس الـ AttributeValue داخل نفس Variant
            RuleFor(x => x.AttributeValueId)
                .Must(values => values.Distinct().Count() == values.Count())
                .WithMessage("Duplicate attribute values are not allowed in the same variant");

            // منع إرسال IDs صفر أو سالبة
            RuleForEach(x => x.AttributeValueId)
                .GreaterThan(0)
                .WithMessage("Invalid AttributeValueId");
        }
    }


    public class VariantAttributeValueDtoValidator
    : AbstractValidator<VariantAttributeValueDto>
    {
        public VariantAttributeValueDtoValidator()
        {
            RuleFor(x => x.AttributeValueId)
                .GreaterThan(0)
                .WithMessage("Invalid AttributeValueId");

           
        }
    }

}
