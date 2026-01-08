using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.DeleteFromBasket
{
    public class DeleteFromBasketCommandValidator:AbstractValidator<DeleteFromBasketCommand>
    {
        public DeleteFromBasketCommandValidator()
        {
            RuleFor(x => x.userid)
            .NotEmpty().WithMessage("User ID is required.")
            .GreaterThan(0).WithMessage("User ID must be a positive integer.");

            RuleFor(x => x.Productid)
                .NotEmpty().WithMessage("Product ID is required.")
                .GreaterThan(0).WithMessage("Product ID must be a positive integer.");
        }
    }
}
