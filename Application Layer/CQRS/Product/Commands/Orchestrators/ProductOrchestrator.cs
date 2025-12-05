using Domain_Layer.DTOs.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Product.Commands.Orchestrators
{
    public class ProductOrchestrator
    {
        // Commands
        public IRequest<ProductDto>? CreateProductCommand { get; set; }
        public IRequest<ProductDto>? UpdateProductCommand { get; set; }
        public IRequest<bool>? DeleteProductCommand { get; set; }

        // Queries
        public IRequest<ProductDto>? GetProductByIdQuery { get; set; }
        public IRequest<IEnumerable<ProductDto>>? GetAllProductsQuery { get; set; }

        public ProductOrchestrator()
        {
        }
    }
}
