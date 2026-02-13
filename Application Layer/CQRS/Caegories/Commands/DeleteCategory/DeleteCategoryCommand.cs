using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Caegories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand(int Id)
         : ICommand<RequestRespones<bool>>;
    public class DeleteCategoryHandler
    : IRequestHandler<DeleteCategoryCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;
        private readonly IMemoryCache memoryCache;
        const string cacheKey = "Categories_All_Default";
        public DeleteCategoryHandler(IGenaricRepository<Category> categoryRepo, IMemoryCache memoryCache
)
        {
            _categoryRepo = categoryRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
           
            
                
                var category = await _categoryRepo.GetByCriteriaAsync(c => c.Id == request.Id);
                if (category == null)
                    return RequestRespones<bool>.Fail($"Category with Id {request.Id} not found.", 404);

        
               _categoryRepo.Delete(category);

           
                await _categoryRepo.SaveChanges();

            memoryCache.Remove(cacheKey);

            return RequestRespones<bool>.Success(true, 200, "Category deleted successfully.");
            
            
        }
    }

}
