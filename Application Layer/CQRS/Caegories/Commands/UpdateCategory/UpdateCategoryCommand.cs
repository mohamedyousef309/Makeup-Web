using Domain_Layer.DTOs._ِCategoryDtos;
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

namespace Application_Layer.CQRS.Caegories.Commands.UpdateCategory
{
    public record UpdateCategoryCommand(UpdateCategoryDto UpdateCategoryDto)
        : ICommand<RequestRespones<CategoryDto>>;
    public class UpdateCategoryHandler
    : IRequestHandler<UpdateCategoryCommand, RequestRespones<CategoryDto>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;

        public UpdateCategoryHandler(IGenaricRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<RequestRespones<CategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            
            
                var dto = request.UpdateCategoryDto;

                var category = await _categoryRepo.GetByCriteriaAsync(x => x.Id == dto.Id);
                if (category == null)
                    return RequestRespones<CategoryDto>.Fail($"Category with Id {dto.Id} not found.", 404);

                var updatedCategory = new Category
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Description = dto.Description
                };

                _categoryRepo.SaveInclude(updatedCategory);
                await _categoryRepo.SaveChanges();

                // 3️⃣ رجّع DTO
                var resultDto = new CategoryDto
                {
                    Id = updatedCategory.Id,
                    Name = updatedCategory.Name,
                    Description = updatedCategory.Description
                };

                return RequestRespones<CategoryDto>.Success(resultDto, 200, "Category updated successfully.");
            
           
        }
    }
}
