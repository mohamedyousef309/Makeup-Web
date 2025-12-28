using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Commands.UpdateProductStock
{
    public record UpdateProductStockCommand(int ProductId, int NewStock) : ICommand<bool>;

    public class UpdateProductStockCommandHandler : IRequestHandler<UpdateProductStockCommand, bool>
    {
        private readonly IGenaricRepository<Product> productRepository;

        public UpdateProductStockCommandHandler(IGenaricRepository<Domain_Layer.Entites.Product> productRepository)
        {
            this.productRepository = productRepository;
        }
        public async Task<bool> Handle(UpdateProductStockCommand request, CancellationToken cancellationToken)
        {
            var product = await productRepository.GetByCriteriaQueryable(x=>x.Id==request.ProductId).FirstOrDefaultAsync();

            if (product == null)
            {
                return false; 
            }
            product.Stock = request.NewStock;
             productRepository.Update(product);
            return true; 
        }
    }


}
