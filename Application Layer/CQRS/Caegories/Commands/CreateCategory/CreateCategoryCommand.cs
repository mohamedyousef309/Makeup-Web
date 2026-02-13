using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Application_Layer.CQRS.Caegories.Commands.CreateCategory
{
    public record CreateCategoryCommand(CreateCategoryDto CreateCategoryDto)
        : ICommand<RequestRespones<bool>>;

    public class CreateCategoryHandler
        : IRequestHandler<CreateCategoryCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;
        private readonly IMemoryCache memoryCache;
        const string cacheKey = "Categories_All_Default";

        public CreateCategoryHandler(IGenaricRepository<Category> categoryRepo,IMemoryCache memoryCache)
        {
            _categoryRepo = categoryRepo;
            this.memoryCache = memoryCache;
        }

        public async Task<RequestRespones<bool>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            
            
                var dto = request.CreateCategoryDto;

                 
                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description
                };

                await _categoryRepo.addAsync(category);
                await _categoryRepo.SaveChanges();

               memoryCache.Remove(cacheKey);


                var resultDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return RequestRespones<bool>.Success(true);
            
            
        }
    }
}
