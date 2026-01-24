using FluentValidation;
using Application_Layer.CQRS.Products.Queries.GetProductsByCategory;

namespace Application_Layer.CQRS.Products.Validators
{
    public class GetProductsByCategoryIdQueryValidator : AbstractValidator<GetProductsByCategoryIdQuery>
    {
        public GetProductsByCategoryIdQueryValidator()
        {
           
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be a positive integer.");

            
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("Page Index must be at least 1.");

            
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page Size must be between 1 and 100.");

          
            RuleFor(x => x.Search)
                .MinimumLength(2).When(x => !string.IsNullOrEmpty(x.Search))
                .WithMessage("Search term must be at least 2 characters long.");

           
            RuleFor(x => x.SortDir)
                .Must(x => x == null || x.ToLower() == "asc" || x.ToLower() == "desc")
                .WithMessage("Sort direction must be either 'asc' or 'desc'.");
        }
    }
}