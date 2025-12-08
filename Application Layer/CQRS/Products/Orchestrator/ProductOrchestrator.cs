using Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Queries;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Orchestrator
{
    public class ProductOrchestrator : IProductOrchestrator
    {
        private readonly IMediator _mediator;

        public ProductOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestRespones<ProductDto>> CreateProduct(CreateProductDto dto)
        {
            var command = new CreateProductCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                Variants = dto.Variants
            };

            return await _mediator.Send(command);
        }

        public async Task<RequestRespones<ProductDto>> UpdateProduct(UpdateProductDto dto)
        {
            var command = new UpdateProductCommand
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive,
                Variants = dto.Variants
            };

            return await _mediator.Send(command);
        }

        public async Task<RequestRespones<bool>> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            return await _mediator.Send(command);
        }

        public async Task<RequestRespones<List<ProductDto>>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            return await _mediator.Send(query);
        }

        public async Task<RequestRespones<ProductDto>> GetProductById(int id)
        {
            var query = new GetProductByIdQuery(id);
            return await _mediator.Send(query);
        }
    }
}
