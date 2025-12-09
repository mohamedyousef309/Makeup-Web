using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Caegories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery()
         : IRequest<RequestRespones<List<CategoryDto>>>;
    public class GetAllCategoriesHandler
      : IRequestHandler<GetAllCategoriesQuery, RequestRespones<List<CategoryDto>>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;

        public GetAllCategoriesHandler(IGenaricRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<RequestRespones<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = _categoryRepo.GetAll().ToList();

                var dtoList = categories.Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToList();

                return RequestRespones<List<CategoryDto>>.Success(dtoList, 200, "Categories loaded successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<List<CategoryDto>>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
