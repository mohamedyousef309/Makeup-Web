using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.AddProductToCart
{
    public class AddProductToCartCommandValidator:AbstractValidator<AddProductToCartCommand>
    {
        public AddProductToCartCommandValidator()
        {
            RuleFor(x => x) // هذا على الـ command نفسه
        .NotNull()
        .WithMessage("Command cannot be null.");

            RuleFor(x => x.userid)
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.productid)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("ProductName is required.")
                .MaximumLength(200)
                .WithMessage("ProductName cannot exceed 200 characters.");

            RuleFor(x => x.ProductPrice)
                .GreaterThan(0)
                .WithMessage("ProductPrice must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");
        }
    }
}
