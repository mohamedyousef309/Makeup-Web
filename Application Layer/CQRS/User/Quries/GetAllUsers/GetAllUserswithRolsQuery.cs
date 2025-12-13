using Domain_Layer.DTOs;
using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.DTOs.UserDTOS;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Quries.GetAllUsers
{
    public record GetAllUserswithRolsQuery(int pageSize, int pageindex, string? sortby = "id"
        , string? sortdirc = "desc",
        string? search = null) :IRequest<RequestRespones<PaginatedListDto<UserToReturnWithRolsDTo>>>;

    public class GetAllUsersQueryHandler : BaseQueryHandler, IRequestHandler<GetAllUserswithRolsQuery, RequestRespones<PaginatedListDto<UserToReturnWithRolsDTo>>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;

        public GetAllUsersQueryHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<PaginatedListDto<UserToReturnWithRolsDTo>>> Handle(GetAllUserswithRolsQuery request, CancellationToken cancellationToken)
        {
            var users =  genaricRepository.GetAll().Select(x => new UserToReturnWithRolsDTo
            {
                Id = x.Id,
                Username = x.Username,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Picture = x.Picture,
                UserAddress = x.UserAddress,
                UserPermissions = x.userPermissions.Select(UP => new UserPermissionsDTo
                {
                    PermissionId = UP.Id,
                    PermissionName = UP.permission.Name
                }).ToList(),
                UserRoles = x.UserRoles.Select(UP => new UserRolsDTo
                {
                    RoleId = UP.Id,
                    RoleName = UP.role.Name
                }).ToList()
            }).AsQueryable();

            if (!users.Any())
            {
                return RequestRespones<PaginatedListDto<UserToReturnWithRolsDTo>>.Fail("There is no Users", 400);
            }

            if (!string.IsNullOrEmpty( request.search))
            {
                users =ApplySearch(users, request.search, x => x.Username);
            }

            var sortColumns = new Dictionary<string, System.Linq.Expressions.Expression<Func<UserToReturnWithRolsDTo, object>>>(StringComparer.OrdinalIgnoreCase)
            {
                { "id", x => x.Id },
                { "username", x => x.Username },
                { "email", x => x.Email },
                { "phonenumber", x => x.PhoneNumber },
                { "useraddress", x => x.UserAddress},


            };
            users = ApplySorting(users, request.sortby, request.sortdirc, sortColumns);



            users = ApplayPagination(users, request.pageindex, request.pageSize);

            var totalCount = await genaricRepository.CountAsync();


            var usersList = await users.ToListAsync(cancellationToken);


            var result = new PaginatedListDto<UserToReturnWithRolsDTo>
            {
                Items = usersList,
                PageNumber = request.pageindex,
                PageSize =request.pageSize ,
                TotalCount = totalCount
            };

            return RequestRespones<PaginatedListDto<UserToReturnWithRolsDTo>>.Success(result);


        }
    }
}
