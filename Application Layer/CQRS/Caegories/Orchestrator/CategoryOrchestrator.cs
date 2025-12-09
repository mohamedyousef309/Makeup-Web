using Application_Layer.CQRS.Caegories.Commands.CreateCategory;
using Application_Layer.CQRS.Caegories.Commands.DeleteCategory;
using Application_Layer.CQRS.Caegories.Commands.UpdateCategory;
using Application_Layer.CQRS.Caegories.Queries.GetAllCategories;
using Application_Layer.CQRS.Caegories.Queries.GetCAtegoryById;
using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Caegories.Orchestrator
{
    public class CategoryOrchestrator
    {
        private readonly IMediator _mediator;

        public CategoryOrchestrator(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔹 Create
        public async Task<RequestRespones<CategoryDto>> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var command = new CreateCategoryCommand(dto);
            return await _mediator.Send(command);
        }

        // 🔹 Update
        public async Task<RequestRespones<CategoryDto>> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var command = new UpdateCategoryCommand(dto);
            return await _mediator.Send(command);
        }

        // 🔹 Delete
        public async Task<RequestRespones<bool>> DeleteCategoryAsync(int id)
        {
            var command = new DeleteCategoryCommand(id);
            return await _mediator.Send(command);
        }

        // 🔹 Get All
        public async Task<RequestRespones<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            var query = new GetAllCategoriesQuery();
            return await _mediator.Send(query);
        }

        // 🔹 Get By Id
        public async Task<RequestRespones<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            return await _mediator.Send(query);
        }
    }
}
