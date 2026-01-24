using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
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
        public UpdateProductVariantStockCommandHandler(IGenaricRepository<ProductVariant>  genaricRepository)
        {
            GenaricRepository = genaricRepository;
        }

        public IGenaricRepository<ProductVariant> GenaricRepository { get; }

        public async Task<RequestRespones<bool>> Handle(UpdateProductVariantStockCommand request, CancellationToken cancellationToken)
        {
            var ProductVariant = GenaricRepository.GetByIdQueryable(request.ProductVaraintid).FirstOrDefault();

            if (ProductVariant==null)
            {
                return RequestRespones<bool>.Fail("There is no Variant with this id", 404);
            }

            ProductVariant.Stock=request.Newstock;

            GenaricRepository.SaveInclude(ProductVariant,nameof(ProductVariant.Stock));

            await GenaricRepository.SaveChanges();

            return RequestRespones<bool>.Success(true);

        }
    }




}
