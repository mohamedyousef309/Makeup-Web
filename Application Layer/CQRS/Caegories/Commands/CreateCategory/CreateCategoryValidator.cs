using FluentValidation;

namespace Application_Layer.CQRS.Caegories.Commands.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.CreateCategoryDto.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.CreateCategoryDto.Description)
                .MaximumLength(250).WithMessage("Description must not exceed 250 characters.");
        }
    }
}
