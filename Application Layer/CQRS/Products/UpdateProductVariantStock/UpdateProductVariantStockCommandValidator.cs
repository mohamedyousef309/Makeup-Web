using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.UpdateProductVariantStock
{
    public class UpdateProductVariantStockCommandValidator: AbstractValidator<UpdateProductVariantStockCommand>
    {
        public UpdateProductVariantStockCommandValidator()
        {
            RuleFor(x => x.ProductVaraintid)
           .GreaterThan(0)
           .WithMessage("Product variant id must be greater than 0");

            RuleFor(x => x.Newstock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock cannot be negative");
        }
    }
}
