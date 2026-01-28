using Domain_Layer.DTOs;
using Domain_Layer.Entites;
using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application_Layer.CQRS.Attributes.Quries.GetAttributes
{
    public record GetAttributesQuery(
        int PageNumber = 1,
        int PageSize = 10
    ) : ICommand<RequestRespones<PaginatedListDto<string>>>;

    public class GetAttributesQueryHandler :BaseQueryHandler, IRequestHandler<GetAttributesQuery, RequestRespones<PaginatedListDto<string>>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository;

        public GetAttributesQueryHandler(IGenaricRepository<Domain_Layer.Entites.Attribute> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<PaginatedListDto<string>>> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
        {
            var query = genaricRepository.GetAll().Select(a => a.Name).Distinct();

            var totalCount = await query.CountAsync(cancellationToken);

            if (totalCount == 0)
                return RequestRespones<PaginatedListDto<string>>
                    .Fail("There is no attributes yet", 404);

            var pagedQuery = ApplayPagination(query, request.PageNumber, request.PageSize);

            var items = await pagedQuery.ToListAsync(cancellationToken);

            var result = new PaginatedListDto<string>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };

            return RequestRespones<PaginatedListDto<string>>
                .Success(result, 200);

        }
    }
}
