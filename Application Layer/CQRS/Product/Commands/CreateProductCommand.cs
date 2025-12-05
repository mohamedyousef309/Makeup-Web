using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Interfaces.ServiceInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Product.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public CreateProductDto Product { get; set; } = default!;
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
    {
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var createdProduct = await _productService.CreateProductAsync(request.Product);
            return createdProduct;
        }
    }
}
