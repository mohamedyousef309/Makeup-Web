using Application_Layer.CQRS.Caegories.Commands.CreateCategory;
using Application_Layer.CQRS.Caegories.Commands.DeleteCategory;
using Application_Layer.CQRS.Caegories.Commands.UpdateCategory;
using Application_Layer.CQRS.Caegories.Queries.GetAllCategories;
using Application_Layer.CQRS.Caegories.Queries.GetCategoriesLookupQuery;
using Application_Layer.CQRS.Caegories.Queries.GetCAtegoryById;
using Application_Layer.CQRS.Caegories.Queries.GetCategoriesLookupQuery; // تم إضافة الـ namespace الجديد
using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.ViewModels.CategoriesViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery());

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new List<CategoryViewModel>());
            }

            var model = result.Data.Select(dto => new CategoryViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description
            }).ToList();

            return View(model);
        }

       
        [HttpGet]
        public async Task<IActionResult> GetLookupList()
        {
           
            var result = await _mediator.Send(new GetCategoryLookupQuery());

            if (!result.IsSuccess) return BadRequest(result.Message);

            
            return Json(result.Data);
        }

        
        [HttpGet]
        public IActionResult Create() => View(new CreateCategoryViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new CreateCategoryDto
            {
                Name = model.Name,
                Description = model.Description
            };

            var result = await _mediator.Send(new CreateCategoryCommand(dto));

            if (result.IsSuccess)
            {
                TempData["SuccessMessage"] = "Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (!result.IsSuccess) return NotFound();

            var model = new UpdateCategoryViewModel
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Description = result.Data.Description
            };

            return View(model);
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateCategoryViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new UpdateCategoryDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description
            };

            var result = await _mediator.Send(new UpdateCategoryCommand(dto));

            if (result.IsSuccess) return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, result.Message);
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));

            if (result.IsSuccess)
                return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }
    }
}