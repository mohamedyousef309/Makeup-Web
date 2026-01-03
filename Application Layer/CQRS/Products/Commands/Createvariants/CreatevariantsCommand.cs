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
    public record CreatevariantsCommand(IEnumerable<UpdateProductVariantDto> UpdateProductVariantDtos):ICommand<RequestRespones<bool>>;

    public class CreatevariantsCommandHandler : IRequestHandler<CreatevariantsCommand, RequestRespones<bool>>
{
        private readonly IGenaricRepository<ProductVariant> genaricRepository;

        public CreatevariantsCommandHandler(IGenaricRepository<ProductVariant> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public Task<RequestRespones<bool>> Handle(CreatevariantsCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
