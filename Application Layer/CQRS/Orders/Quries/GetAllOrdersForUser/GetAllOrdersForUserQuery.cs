using Domain_Layer.DTOs;
using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Order;
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

namespace Application_Layer.CQRS.Orders.Quries.GetAllOrdersForUser
{
    public record GetAllOrdersForUserQuery(int userid, int pageSize=10, int pageindex=1, string? sortby = "id"
        , string? sortdirc = "desc",
        string? search = null) :ICommand<RequestRespones<PaginatedListDto<OrderToReturnDto>>>;

    public class GetAllOrdersForUserQueryHandler :BaseQueryHandler ,IRequestHandler<GetAllOrdersForUserQuery, RequestRespones<PaginatedListDto<OrderToReturnDto>>>
    {
        private readonly IGenaricRepository<Order> genaricRepository;

        public GetAllOrdersForUserQueryHandler(IGenaricRepository<Order> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<PaginatedListDto<OrderToReturnDto>>> Handle(GetAllOrdersForUserQuery request, CancellationToken cancellationToken)
        {

            var orders =   genaricRepository.GetAll().Where(x=>x.UserId==request.userid).Select(x => new OrderToReturnDto
            {
                orderid = x.Id,
                status = x.status.ToString(),
                BuyerEmail = x.BuyerEmail,               
                orderDate = x.orderDate,
            }).AsQueryable();

           
            if (request.search!=null)
            {
                orders=ApplySearch(orders, request.search, o => o.BuyerEmail, o => o.status);
                
            }

            var sortColumns = new Dictionary<string, System.Linq.Expressions.Expression<Func<OrderToReturnDto, object>>>
            {
                { "id", o => o.orderid },
                { "orderDate", o => o.orderDate },
                { "status", o => o.status },
                { "BuyerEmail", o => o.BuyerEmail }
            };

            orders = ApplySorting(orders, request.sortby, request.sortdirc, sortColumns);

            var totalCount = await orders.CountAsync();
            if (totalCount == 0)
                return RequestRespones<PaginatedListDto<OrderToReturnDto>>.Fail("No orders found", 404);

            orders = ApplayPagination(orders, request.pageindex, request.pageSize);


            var result = new PaginatedListDto<OrderToReturnDto>
            {
                PageNumber = request.pageindex,
                PageSize = request.pageSize,
                TotalCount = totalCount,
                Items = await orders.ToListAsync()
            };

            return RequestRespones<PaginatedListDto<OrderToReturnDto>>.Success(result);

        }
    }
}
