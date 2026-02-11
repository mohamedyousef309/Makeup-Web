using Application_Layer.CQRS.Attributes.Quries.GetAttributesWithValues;
using Application_Layer.CQRS.Products.Commands.AddAttrputeToVariant;
using Application_Layer.CQRS.Products.Commands.Createvariants;
using Application_Layer.CQRS.Products.Commands.RemoceAttrputeFromVariant;
using Application_Layer.CQRS.Products.Commands.UpdateVariants;
using Application_Layer.CQRS.Products.Commands.UpdateVariantStock;
using Application_Layer.CQRS.Products.Queries;
using Application_Layer.CQRS.Products.Queries.GetProductVariantsByProductid;
using Application_Layer.CQRS.Products.Queries.GetVariantsWithValuesbyVariantid;
using Application_Layer.CQRS.Products.UpdateProductVariantStock;
using Domain_Layer.DTOs;
using Domain_Layer.DTOs.Attribute;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.ViewModels.ProductsViewModels;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.AddAttrputeToVariant;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.AddVariantToproduct;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.ProductsVariantViewModel;
using Domain_Layer.ViewModels.ProductsViewModels.ProductsVariants.UpdateProductsVariantViewModel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Makeup_Web.Controllers
{
    public class ProductVariantController : Controller
    {
        private readonly IMediator _mediator;

        public ProductVariantController(IMediator mediator)
        {
            this._mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }
        #region Queries

        public async Task<IActionResult> GetAllVariants(int productId, int pageIndex = 1, int pageSize = 10, string? sortBy = "id", string? sortDir = "asc", string? search = null)
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
                Stock = dto.Stock
            };

            return View(model);
        }

        #endregion

        #region Commands
        [HttpGet]
        public async Task<IActionResult> AddVariantToProduct(int productId)
        {
            var model = new AddVariantToproductViewModle
            {
                ProductId = productId
            };

            var attributesResult = await _mediator.Send(new GetAttributesWithValuesQuery());
            if (attributesResult.IsSuccess)
            {
                ViewBag.Attributes = attributesResult.Data;
            }
            else
            {
                ViewBag.Attributes = new List<AttributeWithValueDTo>();
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddVariantToProduct(AddVariantToproductViewModle model)
        {
            if (!ModelState.IsValid)
            {
                var attributesResult = await _mediator.Send(new GetAttributesWithValuesQuery());
                if (attributesResult.IsSuccess)
                {
                    ViewBag.Attributes = attributesResult.Data;
                }
                else
                {
                    ViewBag.Attributes = new List<AttributeWithValueDTo>();
                }

                return View(model);
            }

            var result = await _mediator.Send(new CreatevariantsCommand(
                model.ProductId,
                model.CreateProductVariants));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;

                var attributesResult = await _mediator.Send(new GetAttributesWithValuesQuery());
                if (attributesResult.IsSuccess)
                {
                    ViewBag.Attributes = attributesResult.Data;
                }
                else
                {
                    ViewBag.Attributes = new List<AttributeWithValueDTo>();
                }

                return View(model);
            }

            TempData["SuccessMessage"] = "Variants added successfully!";
            return RedirectToAction("Edit", "Products", new { id = model.ProductId });
        }


        [HttpGet]
        public async Task<IActionResult> UpdateVariant(int VariantId)
        {
            var result = await _mediator.Send(new GetVariantsWithValuesbyVariantidQuery(VariantId));
            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return RedirectToAction("Index");
            }


            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVariant(UpdateProductVariantViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            var result = await _mediator.Send(new UpdateProductVariantCommand(
                model.Id,
                model.Price,
                model.VariantName,
                model.Stock,
                model.ImageFile
            ));

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            return RedirectToAction("Edit", "Products", new { id = model.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateVariantsStock([FromBody] UpdateProdcutVariantStockViewModle Modle)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data" });
            }

            var result = await _mediator.Send(new UpdateProductVariantStockCommand(
                Modle.ProductVariantId,
                Modle.NewStock
            ));

            if (result.IsSuccess)
                return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });
        }


        [HttpPost]
        public async Task<IActionResult> DeleteVariant(int id, int productId)
        {
            var result = await _mediator.Send(new DeleteProductVariantCommand(id));

            if (!result.IsSuccess)
                TempData["ErrorMessage"] = result.Message;

            return RedirectToAction(nameof(GetAllVariants), new { productId });
        } 


        [HttpPost]
        public async Task<IActionResult> AddAttributeValueIdToVariant(AddAttrputeToVariantViewModle Modle) 
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data" });

            }

            var result = await _mediator.Send
                (new AddAttrputeToVariantCommand(Modle.ProductVariantId, Modle.AttributeValueId));


            if (result.IsSuccess)
                return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });

        }

        [HttpPost]
        public async Task<IActionResult> RemoveAttributeValueIdFromVariant([FromBody]AddAttrputeToVariantViewModle Modle) 
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data" });

            }

            var result = await _mediator.Send
                (new RemoceAttrputeFromVariantCommand(Modle.ProductVariantId, Modle.AttributeValueId));


            if (result.IsSuccess)
                return Json(new { success = true, message = result.Message });

            return Json(new { success = false, message = result.Message });

        }

        //[HttpPost]
        //public async Task<IActionResult> AddAttrputeToProductVariant(AddAttrputeToVariantViewModle Modle) 
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new { success = false, message = "Invalid data" });
        //    }
        //    var result = await _mediator.Send
        //        (new AddValueToVariantCommand(Modle.ProductVariantId, Modle.AttributeValueId));
        //    if (result.IsSuccess)
        //        return Json(new { success = true, message = result.Message });
        //    return Json(new { success = false, message = result.Message });
        //}

        #endregion
    }
}