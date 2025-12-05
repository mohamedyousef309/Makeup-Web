using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Product.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Product.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Product.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Product.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
            RuleFor(x => x.Product.CategoryId).GreaterThan(0).WithMessage("CategoryId is required.");
        }
    }
}
