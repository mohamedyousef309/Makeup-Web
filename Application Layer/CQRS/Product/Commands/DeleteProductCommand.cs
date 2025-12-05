using Domain_Layer.Interfaces.ServiceInterfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Product.Commands
{
    public class DeleteProductCommand : IRequest<bool>
    {
        public int ProductId { get; set; }
    }
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var deleted = await _productService.DeleteProductAsync(request.ProductId);
            return deleted;
        }
    }
}
