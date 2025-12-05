using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Product.Queries
{
    public record GetAllProductsQuery : IRequest<RequestRespones<IEnumerable<ProductDto>>>;
    
      
    
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, RequestRespones<IEnumerable<ProductDto>>>
    {
        private readonly IProductService _productService;

        public GetAllProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<RequestRespones<IEnumerable<ProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _productService.GetAllAsync();
            if (!products.Any())
            {
                return RequestRespones<IEnumerable<ProductDto>>.Fail("No products found.", 404);

            }
            return RequestRespones<IEnumerable<ProductDto>>.Success(products);
        }
    }
}
