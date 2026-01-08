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
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.Productid)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0.");

            RuleFor(x => x.ProductName)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(200)
                .WithMessage("Product name must not exceed 200 characters.");

            RuleFor(x => x.ProductPrice)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .LessThanOrEqualTo(100)
                .WithMessage("Quantity must be between 1 and 100.");

        }
    }
}
