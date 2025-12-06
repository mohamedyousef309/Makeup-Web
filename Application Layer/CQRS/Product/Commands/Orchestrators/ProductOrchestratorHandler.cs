using Domain_Layer.DTOs.ProductDtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Product.Commands.Orchestrators
{
    public class ProductOrchestratorHandler
    {
        private readonly IMediator _mediator;

        public ProductOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ProductDto?> Handle(ProductOrchestrator orchestrator, CancellationToken cancellationToken)
        {
            //if (orchestrator.CreateProductCommand != null)
            //{
            //    return await _mediator.Send(orchestrator.CreateProductCommand, cancellationToken);
            //}

            //if (orchestrator.UpdateProductCommand != null)
            //{
            //    return await _mediator.Send(orchestrator.UpdateProductCommand, cancellationToken);
            //}

            if (orchestrator.DeleteProductCommand != null)
            {
                bool deleted = await _mediator.Send(orchestrator.DeleteProductCommand, cancellationToken);
                // Return null for delete, or you can wrap in custom response
                return null;
            }

            if (orchestrator.GetProductByIdQuery != null)
            {
                return await _mediator.Send(orchestrator.GetProductByIdQuery, cancellationToken);
            }

            if (orchestrator.GetAllProductsQuery != null)
            {
                // This returns IEnumerable<ProductDto>, you may wrap it if needed
                var products = await _mediator.Send(orchestrator.GetAllProductsQuery, cancellationToken);
                return null; // Or create custom response if needed
            }

            return null;
        }
    }
}
