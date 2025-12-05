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
    public class GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {
        // No parameters needed
    }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetAllProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync();
            return products;
        }
    }
}
