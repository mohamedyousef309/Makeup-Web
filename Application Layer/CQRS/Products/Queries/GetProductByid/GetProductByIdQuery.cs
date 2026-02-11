using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Queries.GetProductByid
{
    public record GetProductByidQuery(int Productid): IRequest<RequestRespones<ProductDto>>;

    public class GetProductByidQueryHandler : IRequestHandler<GetProductByidQuery, RequestRespones<ProductDto>>
    {
        private readonly IGenaricRepository<Product> genaricRepository;

        public GetProductByidQueryHandler(IGenaricRepository<Domain_Layer.Entites.Product> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<ProductDto>> Handle(GetProductByidQuery request, CancellationToken cancellationToken)
        {
            var product = await genaricRepository.GetByCriteriaQueryable(x=>x.Id==request.Productid)
                .Select(p=> new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    ImageUrl = p.ImageUrl,
                    VariantsDto = p.Variants.Select(v=> new VariantDbDto
                    {
                        id=v.Id,
                        Stock=v.Stock,
                        price=v.Price,
                        ImageUrl =v.ImageUrl,
                        VariantName=v.VariantName,

                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);

            if (product==null)
            {
                return RequestRespones<ProductDto>.Fail("Product Not Found", 404);
            }

            return RequestRespones<ProductDto>.Success(product);


        }

    }
}
