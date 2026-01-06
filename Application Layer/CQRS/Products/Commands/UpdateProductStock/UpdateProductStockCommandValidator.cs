using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.UpdateProductStock
{
    public class UpdateProductStockCommandValidator:AbstractValidator<UpdateProductStockCommand>
    {
        public UpdateProductStockCommandValidator()
        {
            RuleFor(x => x.ProductId)
               .GreaterThan(0)
               .WithMessage("ProductId must be a valid positive number.");

            RuleFor(x => x.NewStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("NewStock cannot be negative.");
        }
    }
}
