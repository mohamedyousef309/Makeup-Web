using Domain_Layer.Entites.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Commands.CreatOrder
{
    public class OrderItemsValidator:AbstractValidator<OrderItems>
    {
        public OrderItemsValidator()
        {
            RuleFor(x => x.ProductName)
               .NotEmpty()
               .MaximumLength(200);

            RuleFor(x => x.PictureUrl)
                .NotEmpty();

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero");
        }

    }
}
