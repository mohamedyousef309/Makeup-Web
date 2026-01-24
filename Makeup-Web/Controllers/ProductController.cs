using Application_Layer.CQRS.Caegories.Queries.GetCategoriesLookupQuery;
using Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Commands.CreateProduct;
using Application_Layer.CQRS.Products.Commands.Orchestrators.AddProductWithVariants;
using Application_Layer.CQRS.Products.Commands.UpdateProduct;
using Application_Layer.CQRS.Products.Queries;
using Application_Layer.CQRS.Products.Queries.GetProductsByIds;
using Application_Layer.CQRS.Products.Queries.GetProductsByCategory; // Added Namespace
using Application_Layer.Orchestrators;
using Domain_Layer.DTOs;
using Domain_Layer.DTOs._ِCategoryDtos;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Respones;
using Domain_Layer.ViewModels.ProductsViewModels;
using Domain_Layer.ViewModels.ProductsViewModels.ListItemViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsViewModel;
using Infastructure_Layer.DynamicRBASystem;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ProductOrchestrator _productOrchestrator;

        public ProductsController(IMediator mediator, ProductOrchestrator productOrchestrator)
        {
            _mediator = mediator;
            _productOrchestrator = productOrchestrator;
        }

        #region Queries (Read Operations)

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetAllProducts(int pageIndex = 1, int pageSize = 10, string? sortBy = "id", string? sortDir = "asc", string? search = null)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(pageSize, pageIndex, sortBy, sortDir, search));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new PaginatedListDto<ProductListItemViewModel> { Items = new List<ProductListItemViewModel>() });
            }

            return View(result.Data);
        }

        /// <summary>
        /// Retrieves products belonging to a specific category with pagination and filtering.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetByCategory(int categoryId, int pageIndex = 1, int pageSize = 10, string? sortBy = "id", string? sortDir = "desc", string? search = null)
        {
            var query = new GetProductsByCategoryIdQuery(categoryId, pageSize, pageIndex, sortBy, sortDir, search);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                // You can return an empty list or redirect depending on your UI preference
                return View("GetAllProducts", new PaginatedListDto<ProductDto> { Items = new List<ProductDto>() });
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (!result.IsSuccess) return NotFound();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetMultipleByIds([FromQuery] IEnumerable<int> ids)
        {
            var result = await _mediator.Send(new GetProductsByIdsQuery(ids));
            return Json(result);
        }

        #endregion

        #region Commands (Write Operations)

        [HttpGet]
        [HasPermission("Products.Create")]
        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Products.Create")]
        public async Task<IActionResult> Create(CreateProductWithVariantsDto model)
        {
            if (!ModelState.IsValid) return View(model);

            var productDto = new CreateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId
            };

            var result = await _mediator.Send(new AddProductWithVariantsOrchstrator(
                productDto,
                model.Variants
            ));

            if (result.IsSuccess) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(model);
        }

        [HttpGet]
        [HasPermission("Products.Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (!result.IsSuccess) return NotFound();

            var productDto = result.Data;
            var viewModel = new UpdateProductViewModel
            {
                Id = productDto.Id,
                Name = productDto.Name,
                Description = productDto.Description,
                CategoryId = productDto.CategoryId,
                IsActive = productDto.IsActive
            };

            var categoriesResult = await _mediator.Send(new GetCategoryLookupQuery());
            if (categoriesResult.IsSuccess)
            {
                ViewBag.Categories = categoriesResult.Data;
            }
            else
            {
                ViewBag.Categories = new List<CategoryLookupDto>();
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Products.Edit")]
        public async Task<IActionResult> Edit(UpdateProductViewModel modle)
        {
            if (!ModelState.IsValid)
            {
                var categoriesResult = await _mediator.Send(new GetCategoryLookupQuery());
                ViewBag.Categories = categoriesResult.IsSuccess ? categoriesResult.Data : new List<CategoryLookupDto>();
                return View(modle);
            }

            var UpdateProductDto = new UpdateProductDto
            {
                Id = modle.Id,
                Name = modle.Name,
                Description = modle.Description,
                ImageFile = modle.Image,
                CategoryId = modle.CategoryId,
                IsActive = modle.IsActive,
            };

            var result = await _mediator.Send(new UpdateProductCommand(UpdateProductDto));
            if (result.IsSuccess) return RedirectToAction(nameof(GetAllProducts));

            var categoriesResult2 = await _mediator.Send(new GetCategoryLookupQuery());
            ViewBag.Categories = categoriesResult2.IsSuccess ? categoriesResult2.Data : new List<CategoryLookupDto>();

            ModelState.AddModelError("", result.Message);
            return View(modle);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (result.IsSuccess)
                return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }

        [HttpPost]
        [HasPermission("Products.Edit")]
        public async Task<IActionResult> LinkVariants(int productId, [FromBody] List<int> variantIds)
        {
            var result = await _productOrchestrator.LinkVariantsToProductAsync(productId, variantIds);

            if (result.IsSuccess)
                return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }

        #endregion

        #region Basket Operations (السلة)

        #endregion
    }
}