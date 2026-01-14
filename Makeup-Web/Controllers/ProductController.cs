using Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Commands.AddProductToCart;
using Application_Layer.CQRS.Products.Commands.CreateProduct;
using Application_Layer.CQRS.Products.Commands.UpdateProduct;
using Application_Layer.CQRS.Products.Commands.UpdateProductStock;
using Application_Layer.CQRS.Products.Queries;
using Application_Layer.CQRS.Products.Queries.GetProductsByIds;
using Domain_Layer.DTOs;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.Respones;
using Domain_Layer.ViewModels.ProductsViewModels.ListItemViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) => _mediator = mediator;

        #region Queries (Read Operations)


        public IActionResult Index()
        {
            return View();
        }
        // Products/GetAllProducts
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
        public IActionResult Create() => View();

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await _mediator.Send(new CreateProductCommand(dto));
            if (result.IsSuccess) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

        
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (!result.IsSuccess) return NotFound();

            
            var updateDto = new UpdateProductDto
            {
                Id = result.Data.Id,
                Name = result.Data.Name,
                Description = result.Data.Description,
                Price = result.Data.Price,
                Stock = result.Data.Stock,
                IsActive = result.Data.IsActive,
                CategoryId = result.Data.CategoryId
            };
            return View(updateDto);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateProductDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await _mediator.Send(new UpdateProductCommand(dto));
            if (result.IsSuccess) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(dto);
        }

       
        [HttpPost]
        public async Task<IActionResult> UpdateStock(int productId, int newStock)
        {
            var result = await _mediator.Send(new UpdateProductStockCommand(productId, newStock));
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return Json(result);
        }

        #endregion

        #region Basket Operations (السلة)

      
     

        #endregion
    }
}