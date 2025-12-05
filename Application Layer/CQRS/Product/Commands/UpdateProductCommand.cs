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
    public class UpdateProductCommand : IRequest<ProductDto>
    {
        public UpdateProductDto Product { get; set; } = default!;
    }
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
    {
        private readonly IProductService _productService;

        public UpdateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var updatedProduct = await _productService.UpdateProductAsync(request.Product);
            return updatedProduct;
        }
    }

}
