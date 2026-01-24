using FluentValidation;
using Application_Layer.CQRS.Orders.Commands.UpdateOrderDetails;

namespace Application_Layer.CQRS.Orders.Validators
{
    public class UpdateOrderDetailsCommandValidator : AbstractValidator<UpdateOrderDetailsCommand>
    {
        public UpdateOrderDetailsCommandValidator()
        {
           
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Invalid Order ID.");

           
            RuleFor(x => x.NewItems)
                .NotEmpty().WithMessage("Order must contain at least one item.")
                .Must(items => items != null && items.Count > 0).WithMessage("Items list cannot be empty.");

           
            RuleForEach(x => x.NewItems).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductVariantId)
                    .GreaterThan(0).WithMessage("Invalid Product Variant ID.");

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be at least 1.");
            });
        }
    }
}