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

namespace Application_Layer.CQRS.Products.Queries.GetRandomImages
{
    public record GetRandomImagesQuery:IRequest<RequestRespones<List<string>>>;

    public class GetRandomImagesQueryHandler : IRequestHandler<GetRandomImagesQuery, RequestRespones<List<string>>>
    {
        private readonly IGenaricRepository<ProductVariant> genaricRepository;

        public GetRandomImagesQueryHandler(IGenaricRepository<Domain_Layer.Entites.ProductVariant> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<List<string>>> Handle(GetRandomImagesQuery request, CancellationToken cancellationToken)
        {
            var images = await genaricRepository.GetAll()
                .Where(pv => !string.IsNullOrEmpty(pv.ImageUrl))
                .Select(pv => pv.ImageUrl!)
                .ToListAsync(cancellationToken);

            if (!images.Any()||images==null)
            {
                return RequestRespones<List<string>>.Fail("There is no Images Yet", 404);
            }

            return RequestRespones<List<string>>.Success(images, 200, "Images retrieved successfully");
        }
    }
}
