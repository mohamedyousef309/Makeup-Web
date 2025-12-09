using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Caegories.Commands.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.UpdateCategoryDto.Id)
                .GreaterThan(0).WithMessage("Invalid category ID.");

            RuleFor(x => x.UpdateCategoryDto.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.UpdateCategoryDto.Description)
                .MaximumLength(250).WithMessage("Description cannot exceed 250 characters.");
        }
    }
}
