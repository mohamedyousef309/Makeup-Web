using Domain_Layer.DTOs;
using Domain_Layer.DTOs.ProductVariantDtos;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Products.Queries
{

    public record GetAllProductVariantsQuery(
        int ProductId,
        int PageSize = 10,
        int PageIndex = 1,
        string? SortBy = "id",
        string? SortDir = "asc",
        string? SearchTerm = null
    ) : IRequest<RequestRespones<PaginatedListDto<ProductVariantDto>>>;

    
    public class GetAllProductVariantsHandler : BaseQueryHandler,
        IRequestHandler<GetAllProductVariantsQuery, RequestRespones<PaginatedListDto<ProductVariantDto>>>
    {
        private readonly IGenaricRepository<ProductVariant> _variantRepo;

        public GetAllProductVariantsHandler(IGenaricRepository<ProductVariant> variantRepo)
        {
            _variantRepo = variantRepo;
        }

        public async Task<RequestRespones<PaginatedListDto<ProductVariantDto>>> Handle(
            GetAllProductVariantsQuery request,
            CancellationToken cancellationToken)
        {
            
            IQueryable<ProductVariant> query = _variantRepo.GetAll()
                .Where(v => v.ProductId == request.ProductId)
                .AsQueryable();

           
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = ApplySearch(query, request.SearchTerm, v => v.VariantName);
            }

            var sortColumns = new Dictionary<string, System.Linq.Expressions.Expression<System.Func<ProductVariant, object>>>
            {
                { "id", v => v.Id },
                { "stock", v => v.Stock }
            };

            query = ApplySorting(query, request.SortBy, request.SortDir, sortColumns);


            var totalCount = await query.CountAsync(cancellationToken);

           
            query = ApplayPagination(query, request.PageIndex, request.PageSize);

            
            var items = await query.Select(v => new ProductVariantDto
            {
                Id = v.Id,
                //VariantName = v.VariantName,
                //VariantValue = v.VariantValue,
                Price=v.Price,
                Stock = v.Stock
            }).ToListAsync(cancellationToken);

         
            var result = new PaginatedListDto<ProductVariantDto>
            {
                Items = items,
                PageNumber = request.PageIndex,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return RequestRespones<PaginatedListDto<ProductVariantDto>>.Success(
                result,
                200,
                "Product variants retrieved successfully."
            );
        }
    }
}
