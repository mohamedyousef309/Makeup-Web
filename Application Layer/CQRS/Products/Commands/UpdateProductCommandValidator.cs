using Application_Layer.CQRS.Products.Commands;
using FluentValidation;

namespace Application_Layer.CQRS.Products.Commands
{
    public class UpdateProductCommandValidator
        : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(x => x.Dto.Id)
                .GreaterThan(0)
                .WithMessage("Product Id is required.");

            RuleFor(x => x.Dto.Name)
                .NotEmpty()
                .WithMessage("Product name is required.");

            RuleFor(x => x.Dto.Price)
                .GreaterThan(0)
                .WithMessage("Price must be greater than zero.");

            RuleFor(x => x.Dto.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Stock cannot be negative.");

            RuleFor(x => x.Dto.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId is required.");
        }
    }
}
