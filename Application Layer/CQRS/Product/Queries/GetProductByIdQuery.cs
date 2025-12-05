using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Interfaces.ServiceInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Product.Queries
{
    public class GetProductByIdQuery : IRequest<ProductDto>
    {
        public int ProductId { get; set; }
    }
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IProductService _productService;

        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(request.ProductId);
            return product;
        }
    }
}
