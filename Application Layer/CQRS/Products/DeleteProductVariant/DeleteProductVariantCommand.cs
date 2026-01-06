using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
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

        public DeleteProductVariantCommandHandler(
            IGenaricRepository<ProductVariant> variantRepo)
        {
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<bool>> Handle(
            DeleteProductVariantCommand request,
            CancellationToken cancellationToken)
        {
            var variant = _variantRepo.GetAll()
                .FirstOrDefault(v => v.Id == request.VariantId);

            if (variant == null)
                return RequestRespones<bool>
                    .Fail("Variant not found", 404);

            _variantRepo.Delete(variant); 
            await _variantRepo.SaveChanges();

            return new RequestRespones<bool>(true);
        }
    }
}
