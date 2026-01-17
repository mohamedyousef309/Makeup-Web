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

namespace Application_Layer.CQRS.Products.Commands.UpdateProductStock
{
    public record UpdateProductStockCommand(int ProductId, int NewStock) : ICommand<RequestRespones<int>>;

    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, RequestRespones<int>>
    {
        private readonly IGenaricRepository<Product> productRepository;

        public UpdateProductStockCommandHandler(IGenaricRepository<Domain_Layer.Entites.Product> productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<RequestRespones<int>> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByCriteriaQueryable(x=>x.Id==request.ProductId).Select(x=>new Product 
            {
                Id=x.Id,
                Stock=x.Stock
            }).FirstOrDefaultAsync();

            if (product == null)
            {
                return RequestRespones<int>.Fail("there is no Product with this id",404); 
            }
            product.Stock = request.NewStock;

            productRepository.SaveInclude(product, nameof(product.Stock));
            return RequestRespones<int>.Success(product.Stock); 
        }
    }


}
