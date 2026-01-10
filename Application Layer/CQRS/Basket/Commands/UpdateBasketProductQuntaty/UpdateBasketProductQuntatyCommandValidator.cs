using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Basket.Commands.UpdateBasketProductQuntaty
{
    public class UpdateBasketProductQuntatyCommandValidator:AbstractValidator<UpdateBasketProductQuntatyCommand>
    {
        public UpdateBasketProductQuntatyCommandValidator()
        {
            RuleFor(x => x.Userid)
           .GreaterThan(0)
           .WithMessage("UserId must be greater than zero.");

            RuleFor(x => x.productid)
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than zero.");

            RuleFor(x => x.newQuntaty)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}
