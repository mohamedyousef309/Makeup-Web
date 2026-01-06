using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Products.Commands
{
 
    public class DeleteProductCommand : ICommand<RequestRespones<bool>>
    {
        public int Id { get; set; }

        public DeleteProductCommand(int id)
        {
            Id = id;
        }
    }

   
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public DeleteProductHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepo.GetByCriteriaAsync(x => x.Id == request.Id);

            if (product == null)
            {
                return RequestRespones<bool>.Fail($"Product with Id {request.Id} not found.", 404);
            }

            _productRepo.Delete(product);

            
            await _productRepo.SaveChanges();

            return RequestRespones<bool>.Success(true, 200, "Product deleted successfully.");
        }
    }
}
