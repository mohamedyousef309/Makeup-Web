using Domain_Layer.DTOs;
using Domain_Layer.DTOs.OrderDTOs;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Entites.Order;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Orders.Quries.GetAllOrders
{
    public record GetAllOrdersQuery(int pageSize, int pageindex, string? sortby = "id"
        , string? sortdirc = "desc",
        string? search = null) :IRequest<RequestRespones<PaginatedListDto<OrderToReturnDto>>>;

    public class GetAllOrdersQueryHandler :BaseQueryHandler ,IRequestHandler<GetAllOrdersQuery, RequestRespones<PaginatedListDto<OrderToReturnDto>>>
    {
        private readonly IGenaricRepository<Order> genaricRepository;

        public GetAllOrdersQueryHandler(IGenaricRepository<Domain_Layer.Entites.Order.Order> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<PaginatedListDto<OrderToReturnDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {

            var orders = genaricRepository.GetAll().Select(x=>new OrderToReturnDto 
            {
                orderid=x.Id,
                status=x.status.ToString(),
                subTotal=x.subTotal,
                orderDate=x.orderDate,
                PhoneNumber=x.PhoneNumber,
                BuyerEmail=x.BuyerEmail,

            }).AsQueryable();

            if (!orders.Any())
            {
                return RequestRespones<PaginatedListDto<OrderToReturnDto>>.Fail("There is no Orders", 404);
            }
            if (request.search!=null) 
            {
                orders= ApplySearch(orders, request.search, x => x.BuyerEmail);

            }

            var sortColumns = new Dictionary<string, Expression<Func<OrderToReturnDto, object>>>(StringComparer.OrdinalIgnoreCase) 
            {
                  { "id", x => x.orderid }
                ,{ "date", x => x.orderDate }
                ,{ "email", x => x.BuyerEmail }
                ,{ "subtotal", x => x.subTotal }
                ,{ "status", x => x.status }
            };

            orders = ApplayPagination(orders, request.pageindex, request.pageSize);

            var totalCount = await genaricRepository.CountAsync();

            var reslut = new PaginatedListDto<OrderToReturnDto>
            {
                Items = await orders.ToListAsync(),
                PageSize = request.pageSize,
                PageNumber = request.pageindex,
                TotalCount = totalCount,
            };

            return RequestRespones<PaginatedListDto<OrderToReturnDto>>.Success(reslut);

        }
    }
}
