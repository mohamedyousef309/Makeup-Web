using Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Commands.Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Commands.CreateProduct;
using Application_Layer.CQRS.Products.Queries;
using Domain_Layer.DTOs;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.ViewModels.ProductsViewModels;
using Domain_Layer.ViewModels.ProductsViewModels.CreateProductsViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.ListItemViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsDetailsViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsVariantViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsVariantViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Makeup_Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // =================== Products Endpoints ===================

        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 10, string? sortBy = "id", string? sortDir = "asc", string? search = null)
        {
            var result = await _mediator.Send(new GetAllProductsQuery(pageSize, pageIndex, sortBy, sortDir, search));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new PaginatedListDto<ProductListItemViewModel> { Items = new List<ProductListItemViewModel>() });
            }

            var viewModel = new PaginatedListDto<ProductListItemViewModel>
            {
                Items = result.Data.Items.Select(p => new ProductListItemViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    IsActive = p.IsActive
                }).ToList(),
                PageNumber = result.Data.PageNumber,
                PageSize = result.Data.PageSize,
                TotalCount = result.Data.TotalCount
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var dto = result.Data;
            var model = new ProductDetailsViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                IsActive = dto.IsActive,
                Variants = dto.Variants?.Select(v => new ProductVariantViewModel
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList() ?? new List<ProductVariantViewModel>()
            };

            return View(model);
        }

        public IActionResult Create() => View(new CreateProductViewModel());

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new CreateProductDto
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                Variants = model.Variants?.Select(v => new CreateProductVariantDto
                {
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList() ?? new List<CreateProductVariantDto>()
            };

            var result = await _mediator.Send(new CreateProductCommand(dto));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var dto = result.Data;
            var model = new UpdateProductViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
                IsActive = dto.IsActive,
                Variants = dto.Variants?.Select(v => new UpdateProductVariantViewModel
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList() ?? new List<UpdateProductVariantViewModel>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new UpdateProductDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                IsActive = model.IsActive,
                Variants = model.Variants?.Select(v => new UpdateProductVariantDto
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList() ?? new List<UpdateProductVariantDto>()
            };

            var result = await _mediator.Send(new UpdateProductCommand(dto));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStock(int productId, int stock)
        {
            var result = await _mediator.Send(new UpdateProductStockCommand(productId, stock));

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromBasket(int productId, int userId)
        {
            var result = await _mediator.Send(new RemoveProductFromBasketCommand(productId, userId));

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Index));
        }

        // =================== ProductVariants Endpoints ===================

        public async Task<IActionResult> Variants(int productId, int pageIndex = 1, int pageSize = 10, string? sortBy = "id", string? sortDir = "asc", string? search = null)
        {
            var result = await _mediator.Send(new GetAllProductVariantsQuery(productId, pageSize, pageIndex, sortBy, sortDir, search));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(new PaginatedListDto<ProductVariantViewModel> { Items = new List<ProductVariantViewModel>() });
            }

            var viewModel = new PaginatedListDto<ProductVariantViewModel>
            {
                Items = result.Data.Items.Select(v => new ProductVariantViewModel
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList(),
                PageNumber = result.Data.PageNumber,
                PageSize = result.Data.PageSize,
                TotalCount = result.Data.TotalCount
            };

            return View(viewModel);
        }

        public async Task<IActionResult> VariantDetails(int id)
        {
            var result = await _mediator.Send(new GetProductVariantByIdQuery(id));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction(nameof(Index));
            }

            var dto = result.Data;
            var model = new ProductVariantViewModel
            {
                Id = dto.Id,
                VariantName = dto.VariantName,
                VariantValue = dto.VariantValue,
                Stock = dto.Stock
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVariant(int productId, UpdateProductVariantViewModel model)
        {
            var dto = new UpdateProductVariantDto
            {
                VariantName = model.VariantName,
                VariantValue = model.VariantValue,
                Stock = model.Stock
            };

            var result = await _mediator.Send(new CreatevariantsCommand(productId, new[] { dto }));

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Variants), new { productId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVariant(int productId, UpdateProductVariantViewModel model)
        {
            var dto = new UpdateProductVariantDto
            {
                Id = model.Id,
                VariantName = model.VariantName,
                VariantValue = model.VariantValue,
                Stock = model.Stock
            };

            var result = await _mediator.Send(new UpdateProductVariantCommand(new[] { dto }));

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Variants), new { productId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteVariant(int id, int productId)
        {
            var result = await _mediator.Send(new DeleteProductVariantCommand(id));

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(Variants), new { productId });
        }
    }
}
