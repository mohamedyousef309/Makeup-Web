using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Product Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Stock).GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");
            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("CategoryId is required.");
        }
    }
}
