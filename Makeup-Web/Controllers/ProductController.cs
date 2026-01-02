using Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Commands.AddProductToCart;
using Application_Layer.CQRS.Products.Commands.Application_Layer.CQRS.Products.Commands;
using Application_Layer.CQRS.Products.Commands.RemoveProductFromBasket;
using Application_Layer.CQRS.Products.Commands.UpdateProductStock;
using Application_Layer.CQRS.Products.Queries;
using Domain_Layer.DTOs.ProductDtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Makeup_Web.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User,Admin,SuperAdmin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllProductsQuery());
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "User,Admin,SuperAdmin")]
        [HttpPost("by-ids")]
        public async Task<IActionResult> GetByIds([FromBody] IEnumerable<int> ids)
        {
            var result = await _mediator.Send(new GetProductsByIdsQuery(ids));
            return StatusCode(result.StatusCode, result);
        }

       
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var result = await _mediator.Send(new CreateProductCommand(dto));
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProductDto dto)
        {
            var result = await _mediator.Send(new UpdateProductCommand(dto));
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPatch("{id:int}/stock")]
        public async Task<IActionResult> UpdateStock(int id, [FromQuery] int newStock)
        {
            var success = await _mediator.Send(
                new UpdateProductStockCommand(id, newStock));

            if (!success)
                return NotFound("Product not found");

            return Ok("Stock updated successfully");
        }

    
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return StatusCode(result.StatusCode, result);
        }

        
        [Authorize(Roles = "User")]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart(
            [FromQuery] int userId,
            [FromQuery] int productId,
            [FromQuery] string productName,
            [FromQuery] decimal productPrice,
            [FromQuery] int quantity)
        {
            var result = await _mediator.Send(
                new AddProductToCartCommand(
                    userId,
                    productId,
                    productName,
                    productPrice,
                    quantity));

            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("remove-from-cart")]
        public async Task<IActionResult> RemoveFromCart(
            [FromQuery] int userId,
            [FromQuery] int productId)
        {
            var result = await _mediator.Send(
                new RemoveProductFromBasketCommand(userId, productId));

            return StatusCode(result.StatusCode, result);
        }
    }
}
