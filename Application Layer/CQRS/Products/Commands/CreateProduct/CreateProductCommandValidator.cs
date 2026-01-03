using Application_Layer.CQRS.Products.Commands.CreateProduct;
using Domain_Layer.DTOs.ProductVariantDtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands
{


    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            // Validate product main fields
            RuleFor(x => x.CreateProductDto.Name)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.CreateProductDto.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.CreateProductDto.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero.");

            RuleFor(x => x.CreateProductDto.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be negative.");

            RuleFor(x => x.CreateProductDto.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than zero.");

           
        }
    }
   
}

