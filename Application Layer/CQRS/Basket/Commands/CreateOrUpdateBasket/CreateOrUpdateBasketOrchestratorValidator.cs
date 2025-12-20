using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.CreateOrUpdateBasket
{
    public class CreateOrUpdateBasketOrchestratorValidator:AbstractValidator<CreateOrUpdateBasketOrchestrator>
    {
        public CreateOrUpdateBasketOrchestratorValidator()
        {
            RuleFor(x => x.userid)
     .GreaterThan(0).WithMessage("The User ID (userid) must be a valid positive number.");

            RuleFor(x => x.Quantity)
                 .GreaterThan(0)
                 .WithMessage("Quantity must be at least 1.")
                 .LessThanOrEqualTo(100)
                 .WithMessage("Quantity cannot exceed 100.");

            RuleFor(x => x.Productid)
                .GreaterThan(0).WithMessage("The Product ID (Productid) must be a valid positive number.");

        }
    }
}
