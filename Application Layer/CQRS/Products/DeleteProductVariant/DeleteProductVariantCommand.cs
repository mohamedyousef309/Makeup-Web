using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; 

namespace Application_Layer.CQRS.Products.Commands.UpdateVariants
{
    
    public record DeleteProductVariantCommand(int VariantId)
        : ICommand<RequestRespones<bool>>;

    
    public class DeleteProductVariantCommandHandler
        : IRequestHandler<DeleteProductVariantCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<ProductVariant> _variantRepo;
        private readonly IMemoryCache memoryCache;

        public DeleteProductVariantCommandHandler(
            IGenaricRepository<ProductVariant> variantRepo,IMemoryCache memoryCache)
        {
            _variantRepo = variantRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(
            DeleteProductVariantCommand request,
            CancellationToken cancellationToken)
        {
            var variant = _variantRepo.GetByCriteriaQueryable(x=>x.Id==request.VariantId).Select(x=> new ProductVariant 
            {
                Id=x.Id,
            })
                .FirstOrDefault();

            if (variant == null)
                return RequestRespones<bool>
                    .Fail("Variant not found", 404);

            string cacheKey = $"ProductDetails_{variant.Id}";


            _variantRepo.Delete(variant); 
            await _variantRepo.SaveChanges();

            memoryCache.Remove(cacheKey);

            return new RequestRespones<bool>(true,"Deleted Successfully");
        }
    }
}
