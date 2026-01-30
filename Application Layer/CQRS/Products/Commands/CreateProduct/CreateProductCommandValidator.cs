using Application_Layer.CQRS.Products.Commands.CreateProduct;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands
{


    public class CreateProductDtoValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .When(x => !string.IsNullOrWhiteSpace(x.Description))
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("Invalid CategoryId.");

            RuleFor(x => x.Productpecture)
                .Must(BeValidImage)
                .When(x => x.Productpecture != null)
                .WithMessage("Invalid image file.");
        }

        private bool BeValidImage(IFormFile file)
        {
            var allowedTypes = new[] { "image/jpeg", "image/png", "image/webp" };
            return allowedTypes.Contains(file.ContentType)
                   && file.Length > 0
                   && file.Length <= 5 * 1024 * 1024; // 5MB
        }
    }

}

