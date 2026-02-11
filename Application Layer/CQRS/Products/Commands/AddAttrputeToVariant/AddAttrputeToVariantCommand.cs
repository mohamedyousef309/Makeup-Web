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

namespace Application_Layer.CQRS.Products.Commands.AddAttrputeToVariant
{
    public record AddAttrputeToVariantCommand(int ProductVariantId, int AttributeValueId) : ICommand<RequestRespones<bool>>;

    public class AddAttrputeToVariantCommandHandler : IRequestHandler<AddAttrputeToVariantCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<VariantAttributeValue> genaricRepository;

        public AddAttrputeToVariantCommandHandler(IGenaricRepository<Domain_Layer.Entites.VariantAttributeValue> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(AddAttrputeToVariantCommand request, CancellationToken cancellationToken)
        {
            var variantAttributeValue = await genaricRepository.GetByCriteriaQueryable(v => v.ProductVariantId == request.ProductVariantId && v.AttributeValueId == request.AttributeValueId).
                FirstOrDefaultAsync();
            if (variantAttributeValue==null)
            {
                return RequestRespones<bool>.Fail(" Varient AllReadyExist", 404);
            }
            var newVariantAttributeValue = new VariantAttributeValue
            {
                ProductVariantId = request.ProductVariantId,
                AttributeValueId = request.AttributeValueId,
            };

           await genaricRepository.addAsync(newVariantAttributeValue);

            await genaricRepository.SaveChanges();

            return RequestRespones<bool>.Success(true);


        }
    }
}
