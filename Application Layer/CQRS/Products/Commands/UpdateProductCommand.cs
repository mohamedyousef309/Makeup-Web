using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input; 

namespace Application_Layer.CQRS.Products.Commands
{
    
    public record UpdateProductCommand(UpdateProductDto Dto)
        : ICommand<RequestRespones<bool>>;

   
    public class UpdateProductHandler
        : IRequestHandler<UpdateProductCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public UpdateProductHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<bool>> Handle(
            UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            var product = _productRepo.GetAll()
                .FirstOrDefault(p => p.Id == dto.Id);

            if (product == null)
                return RequestRespones<bool>
                    .Fail("Product not found", 404);

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;
            product.IsActive = dto.IsActive;

            _productRepo.SaveInclude(product);
            await _productRepo.SaveChanges();

            return RequestRespones<bool>.Success(true, 200, "Product updated successfully");
        }
    }
}
