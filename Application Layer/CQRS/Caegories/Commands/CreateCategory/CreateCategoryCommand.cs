using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;

namespace Application_Layer.CQRS.Caegories.Commands.CreateCategory
{
    public record CreateCategoryCommand(CreateCategoryDto CreateCategoryDto)
        : IRequest<RequestRespones<CategoryDto>>;

    public class CreateCategoryHandler
        : IRequestHandler<CreateCategoryCommand, RequestRespones<CategoryDto>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;

        public CreateCategoryHandler(IGenaricRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<RequestRespones<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dto = request.CreateCategoryDto;

                 
                var category = new Category
                {
                    Name = dto.Name,
                    Description = dto.Description
                };

                await _categoryRepo.addAsync(category);
                await _categoryRepo.SaveChanges();

                
                var resultDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description
                };

                return RequestRespones<CategoryDto>.Success(resultDto, 201, "Category created successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<CategoryDto>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
