using Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Commands.Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Queries;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.ViewModels.ProductsViewModels.CreateProductsViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.ListItemViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsDetailsViewModel;
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

      
        public async Task<IActionResult> Index()
        {
            //var result = await _mediator.Send(new GetAllProductsQuery());

            //if (!result.IsSuccess)
            //{
            //    TempData["ErrorMessage"] = result.Message;
            //    return View(new List<ProductListItemViewModel>());
            //}

            //var viewModel = result.Data.Select(p => new ProductListItemViewModel
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Price = p.Price,
            //    Stock = p.Stock,
            //    IsActive = p.IsActive
            //}).ToList();

            return View(/*viewModel*/);
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
                Variants = model.Variants?.Select(v => new Domain_Layer.DTOs.ProductVariantDtos.CreateProductVariantDto
                {
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList()
            };

            var result = await _mediator.Send(new CreateProductCommand(dto));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to create product");
                return View(model);
            }

            return RedirectToAction("Index");
        }

       
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
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
                Variants = dto.Variants.Select(v => new Domain_Layer.ViewModels.ProductsViewModels.UpdateProductsVariantViewModel.UpdateProductVariantViewModel
                {
                    Id = v.Id, 
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateProductViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var dto = new Domain_Layer.DTOs.ProductDtos.UpdateProductDto
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                CategoryId = model.CategoryId,
                IsActive = model.IsActive,
                Variants = model.Variants?.Select(v => new Domain_Layer.DTOs.ProductVariantDtos.UpdateProductVariantDto
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList()
            };

            var result = await _mediator.Send(new UpdateProductCommand(dto));

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Message ?? "Failed to update product");
                return View(model);
            }

            return RedirectToAction("Index");
        }

      
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            var result = await _mediator.Send(new GetProductByIdQuery(id));
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
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
                Variants = dto.Variants.Select(v => new Domain_Layer.ViewModels.ProductsViewModels.ProductsVariantViewModel.ProductVariantViewModel
                {
                    Id = v.Id,
                    VariantName = v.VariantName,
                    VariantValue = v.VariantValue,
                    Stock = v.Stock
                }).ToList()
            };

            return View(model);
        }
    }
}
