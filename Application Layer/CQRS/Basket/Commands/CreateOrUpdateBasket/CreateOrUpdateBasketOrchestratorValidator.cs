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

            RuleFor(x => x.basketid)
                .NotEmpty().WithMessage("The Basket ID (basketid) is required.")
                .MaximumLength(50).WithMessage("Basket ID cannot exceed 50 characters.");

            RuleFor(x => x.Productid)
                .GreaterThan(0).WithMessage("The Product ID (Productid) must be a valid positive number.");
        }
    }
}
