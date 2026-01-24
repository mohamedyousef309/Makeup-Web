using Application_Layer.CQRS.Products.Queries.GetProductsByCategory;
using Domain_Layer.DTOs;
using Domain_Layer.DTOs.ProductDtos;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application_Layer.CQRS.Products.Queries.GetProductsByCategory
{
    
    public record GetProductsByCategoryIdQuery(
        int CategoryId,
        int PageSize = 10,
        int PageIndex = 1,
        string? SortBy = "id",
        string? SortDir = "desc",
        string? Search = null
    ) : IRequest<RequestRespones<PaginatedListDto<ProductDto>>>;

    public class GetProductsByCategoryIdHandler : BaseQueryHandler, IRequestHandler<GetProductsByCategoryIdQuery, RequestRespones<PaginatedListDto<ProductDto>>>
    {
        private readonly IGenaricRepository<Product> _productRepo;

        public GetProductsByCategoryIdHandler(IGenaricRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<RequestRespones<PaginatedListDto<ProductDto>>> Handle(GetProductsByCategoryIdQuery request, CancellationToken cancellationToken)
        {
            
            var query = _productRepo.GetByCriteriaQueryable(p => p.CategoryId == request.CategoryId && p.IsActive);

            
            if (!string.IsNullOrEmpty(request.Search))
            {
              
                query = ApplySearch(query, request.Search, x => x.Name);
            }

            
            var totalCount = await query.CountAsync(cancellationToken);

            
            var sortColumns = new Dictionary<string, System.Linq.Expressions.Expression<Func<Product, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                { "id", x => x.Id },
                { "name", x => x.Name },
                { "active", x => x.IsActive }
            };
            query = ApplySorting(query, request.SortBy, request.SortDir, sortColumns);

            
            query = ApplayPagination(query, request.PageIndex, request.PageSize);
 
            var productsList = await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name ?? "No Name",
                Description = p.Description,
                ImageUrl = p.ImageUrl ?? "",
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                ProductStock = p.Variants.Sum(v => v != null ? v.Stock : 0),
                Variants = p.Variants.Select(v => new ProductVariantDto
                {
                    Id = v!.Id,
                    ProductId = v.ProductId,
                    VariantName = v.VariantName ?? "",
                    Price = v.Price,
                    Stock = v.Stock
                }).ToList()
            }).ToListAsync(cancellationToken);

            
            var result = new PaginatedListDto<ProductDto>
            {
                Items = productsList,
                PageNumber = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return RequestRespones<PaginatedListDto<ProductDto>>.Success(result, 200, "Retrieved successfully.");
        }
    }
}