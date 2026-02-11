using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.RemoceAttrputeFromVariant
{
    public record RemoceAttrputeFromVariantCommand(int Variantid,int AttributeValueId) :ICommand<RequestRespones<bool>>;

    public class RemoceAttrputeFromVariantCommandHandler : IRequestHandler<RemoceAttrputeFromVariantCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<VariantAttributeValue> genaricRepository;

        public RemoceAttrputeFromVariantCommandHandler(IGenaricRepository<Domain_Layer.Entites.VariantAttributeValue> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(RemoceAttrputeFromVariantCommand request, CancellationToken cancellationToken)
        {
           var VariantAttribute= await genaricRepository.GetByCriteriaQueryable(x=>x.ProductVariantId==request.Variantid && x.AttributeValueId==request.AttributeValueId).FirstOrDefaultAsync(cancellationToken);
            if (VariantAttribute==null)
            {
                return RequestRespones<bool>.Fail("Attribute Not Found For This Variant", 404);
            }
            genaricRepository.Delete(VariantAttribute);
            await genaricRepository.SaveChanges();
            return RequestRespones<bool>.Success(true, 200, "Attribute removed from variant successfully");
        }
    }
}
