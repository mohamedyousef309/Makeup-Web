using Application_Layer.CQRS.Products.Commands.CreateProduct;
using Application_Layer.CQRS.Products.Commands.Createvariants;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.Orchestrators.AddProductWithVariants
{
    public record AddProductWithVariantsOrchstrator(CreateProductDto CreateProductDto, IEnumerable<CreateProductVariantDto?> CreateProductVariantDtos) :ICommand<RequestRespones<bool>>;

    public class AddProductWithVariantsOrchstratorHandler : IRequestHandler<AddProductWithVariantsOrchstrator, RequestRespones<bool>>
    {
        private readonly IMediator mediator;

        public AddProductWithVariantsOrchstratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<RequestRespones<bool>> Handle(AddProductWithVariantsOrchstrator request, CancellationToken cancellationToken)
        {
            var addProductReslut = await mediator.Send(new CreateProductCommand(request.CreateProductDto));

            if (!addProductReslut.IsSuccess || addProductReslut.Data == null)
            {
                return RequestRespones<bool>.Fail(addProductReslut.Message ?? "Failed to add product", addProductReslut.StatusCode);
            }
            if (request.CreateProductVariantDtos != null)
            {
                var addProductVariantsReslut = await mediator.Send(new CreatevariantsCommand(addProductReslut.Data.Id, request.CreateProductVariantDtos!));

                if (!addProductVariantsReslut.IsSuccess)
                {
                    return RequestRespones<bool>.Fail(addProductVariantsReslut.Message ?? "Failed to add variants", addProductVariantsReslut.StatusCode);
                }
            }

           

            return RequestRespones<bool>.Success(true, Message: "Product and its variants added successfully within a single transaction.");
        }


    }
    
}
