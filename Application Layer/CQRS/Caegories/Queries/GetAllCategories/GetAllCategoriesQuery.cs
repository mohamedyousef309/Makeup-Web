using Domain_Layer.DTOs;
using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application_Layer.CQRS.Caegories.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery()
         : IRequest<RequestRespones<PaginatedListDto<CategoryDto>>>;
    public class GetAllCategoriesHandler
      : BaseQueryHandler,IRequestHandler<GetAllCategoriesQuery, RequestRespones<PaginatedListDto<CategoryDto>>>
    {
        private readonly IGenaricRepository<Category> _categoryRepo;

        public GetAllCategoriesHandler(IGenaricRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<RequestRespones<PaginatedListDto<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var categories = _categoryRepo.GetAll().AsQueryable();


                categories = ApplayPagination(categories, 1, 20);

                var count = await categories.CountAsync();

                var result= new PaginatedListDto<CategoryDto> 
                {
                    Items =await  categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                    }).ToListAsync(),
                    PageSize = 20,
                    PageNumber = 1,
                    TotalCount = count
                };
              

                return RequestRespones<PaginatedListDto<CategoryDto>>.Success(result, 200, "Categories loaded successfully.");
            }
            catch (Exception ex)
            {
                return RequestRespones<PaginatedListDto<CategoryDto>>.Fail($"Error: {ex.Message}", 500);
            }
        }
    }
}
