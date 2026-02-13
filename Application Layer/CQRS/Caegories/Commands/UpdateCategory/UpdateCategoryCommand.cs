using Domain_Layer.DTOs._ِCategoryDtos;
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

namespace Application_Layer.CQRS.Caegories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(UpdateCategoryDto UpdateCategoryDto)
        : ICommand<RequestRespones<bool>>;
    public class UpdateCategoryHandler
    : IRequestHandler<UpdateCategoryCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;
        private readonly IMemoryCache memoryCache;
        const string cacheKey = "Categories_All_Default";

        public UpdateCategoryHandler(IGenaricRepository<Category> categoryRepo, IMemoryCache memoryCache)
        {
            _categoryRepo = categoryRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            
            
                var dto = request.UpdateCategoryDto;

                var category = await _categoryRepo.GetByCriteriaQueryable(x => x.Id == dto.Id).FirstOrDefaultAsync();
                if (category == null)
                    return RequestRespones<bool>.Fail($"Category with Id {dto.Id} not found.", 404);

                category.Description = dto.Description;
                category.Name = dto.Name;

                _categoryRepo.SaveInclude(category,nameof(category.Description),nameof(category.Name));
                await _categoryRepo.SaveChanges();

                  memoryCache.Remove(cacheKey);



            return RequestRespones<bool>.Success(true, 200, "Category updated successfully.");
            
           
        }
    }
}
