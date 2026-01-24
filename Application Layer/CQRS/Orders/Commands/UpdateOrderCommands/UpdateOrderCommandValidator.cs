using Application_Layer.CQRS.Orders.Commands.UpdateOrder;
using FluentValidation;

namespace Application_Layer.CQRS.Orders.Validators
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).GreaterThan(0);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(500);
            RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\d+$").WithMessage("Phone number must be digits only.");
            RuleFor(x => x.Status).IsInEnum().WithMessage("Invalid order status.");
        }
    }
}