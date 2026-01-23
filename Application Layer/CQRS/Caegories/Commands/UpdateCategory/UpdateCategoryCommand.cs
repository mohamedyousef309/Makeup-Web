using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

        public UpdateCategoryHandler(IGenaricRepository<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
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

             

                return RequestRespones<bool>.Success(true, 200, "Category updated successfully.");
            
           
        }
    }
}
