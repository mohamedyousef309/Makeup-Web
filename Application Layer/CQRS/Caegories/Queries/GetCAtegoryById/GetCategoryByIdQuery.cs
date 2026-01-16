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

    namespace Application_Layer.CQRS.Caegories.Queries.GetCAtegoryById
    {
        public record GetCategoryByIdQuery(int Id)
             : IRequest<RequestRespones<CategoryDto>>;
        public class GetCategoryByIdHandler
           : IRequestHandler<GetCategoryByIdQuery, RequestRespones<CategoryDto>>
        {
            private readonly IGenaricRepository<Category> _categoryRepo;

            public GetCategoryByIdHandler(IGenaricRepository<Category> categoryRepo)
            {
                _categoryRepo = categoryRepo;
            }

            public async Task<RequestRespones<CategoryDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                  var category= await _categoryRepo.GetByCriteriaQueryable(x=>x.Id==request.Id).Select(x => new CategoryDto
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Description = x.Description
                  }).FirstOrDefaultAsync(cancellationToken);


                if (category == null)
                        return RequestRespones<CategoryDto>.Fail("Category not found.", 404);


                    return RequestRespones<CategoryDto>.Success(category, 200, "Category retrieved successfully.");
                }
                catch (Exception ex)
                {
                    return RequestRespones<CategoryDto>.Fail($"Error: {ex.Message}", 500);
                }
            }
        }
    }
