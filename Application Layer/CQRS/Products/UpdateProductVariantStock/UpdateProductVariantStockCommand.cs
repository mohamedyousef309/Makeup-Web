using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Products.UpdateProductVariantStock
{
    public record UpdateProductVariantStockCommand(int ProductVaraintid, int Newstock):ICommand<RequestRespones<bool>>;

    public class UpdateProductVariantStockCommandHandler:IRequestHandler<UpdateProductVariantStockCommand, RequestRespones<bool>>
    {
        private readonly IMemoryCache memoryCache;

        public UpdateProductVariantStockCommandHandler(IGenaricRepository<ProductVariant>  genaricRepository,IMemoryCache memoryCache)
        {
            GenaricRepository = genaricRepository;
            this.memoryCache = memoryCache;
        }

        public IGenaricRepository<ProductVariant> GenaricRepository { get; }

        public async Task<RequestRespones<bool>> Handle(UpdateProductVariantStockCommand request, CancellationToken cancellationToken)
        {
            var ProductVariant = GenaricRepository.GetByIdQueryable(request.ProductVaraintid).Select(x => new ProductVariant 
            {
                Id= x.Id,
               Stock= x.Stock,
               ProductId= x.ProductId,

            }).FirstOrDefault();

            if (ProductVariant==null)
            {
                return RequestRespones<bool>.Fail("There is no Variant with this id", 404);
            }

            string cacheKey = $"ProductDetails_{ProductVariant.ProductId}";


            ProductVariant.Stock=request.Newstock;

            GenaricRepository.SaveInclude(ProductVariant,nameof(ProductVariant.Stock));

            await GenaricRepository.SaveChanges();

            memoryCache.Remove(cacheKey);

            return RequestRespones<bool>.Success(true);

        }
    }




}
