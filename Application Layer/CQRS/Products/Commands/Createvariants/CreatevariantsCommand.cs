using Domain_Layer.DTOs.ProductVariantDtos;
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

namespace Application_Layer.CQRS.Products.Commands.Createvariants
{
    public record CreatevariantsCommand(int productid,IEnumerable<CreateProductVariantDto> UpdateProductVariantDtos):ICommand<RequestRespones<bool>>;

    public class CreatevariantsCommandHandler : IRequestHandler<CreatevariantsCommand, RequestRespones<bool>>
{
        private readonly IGenaricRepository<ProductVariant> genaricRepository;

        public CreatevariantsCommandHandler(IGenaricRepository<ProductVariant> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(CreatevariantsCommand request, CancellationToken cancellationToken)
        {

            var variants = request.UpdateProductVariantDtos
                .Select(dto =>
                new ProductVariant
                {ProductId = request.productid
                ,Price = dto.Price
                ,Stock = dto.Stock
                ,ProductVariantAttributeValues = dto.AttributeValueId.Select(avId => new VariantAttributeValue
                {
                    AttributeValueId = avId
                }).ToList()
                }).ToList();




            await genaricRepository.AddRangeAsync(variants);

            await genaricRepository.SaveChanges();  

            return new RequestRespones<bool>(true);

        }
    }
}
