using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandValidator:AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusCommandValidator()
        {
            RuleFor(x => x.Orderid)
                .GreaterThan(0)
                .WithMessage("OrderId must be greater than 0");

            RuleFor(x => x.OrderStatus)
                .IsInEnum()
                .WithMessage("Invalid order status");
        }
    }
}
