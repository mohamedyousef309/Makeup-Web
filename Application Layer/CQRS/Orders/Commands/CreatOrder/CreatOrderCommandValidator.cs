using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Commands.CreatOrder
{
    public class CreatOrderCommandValidator:AbstractValidator<CreatOrderCommand>
    {
        public CreatOrderCommandValidator()
        {
            RuleFor(x => x.BuyerEmail)
                .NotEmpty().WithMessage("Buyer email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(500);

            RuleFor(x => x.Deliverycost)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Delivery cost cannot be negative");

            RuleFor(x => x.subTotal)
                .GreaterThan(0)
                .WithMessage("Subtotal must be greater than zero");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Order items are required")
                .Must(items => items.Any())
                .WithMessage("Order must contain at least one item");

            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemsValidator());

        }
    }
}
