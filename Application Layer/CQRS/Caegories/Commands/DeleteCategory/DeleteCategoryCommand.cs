using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Caegories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int Id)
         : IRequest<RequestRespones<bool>>;
    public class DeleteCategoryHandler
    : IRequestHandler<DeleteCategoryCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;

        public DeleteCategoryHandler(IGenaricRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<RequestRespones<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                
                var category = await _categoryRepo.GetByCriteriaAsync(c => c.Id == request.Id);
                if (category == null)
                    return RequestRespones<bool>.Fail($"Category with Id {request.Id} not found.", 404);

        
               _categoryRepo.Delete(category);

           
                await _categoryRepo.SaveChanges();

                return RequestRespones<bool>.Success(true, 200, "Category deleted successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }

}
