using Application_Layer.CQRS.Authantication.Quries.GetPermissionsByUserid;
using Application_Layer.CQRS.Authantication.Quries.GetRolesByUserid;
using Application_Layer.CQRS.User.Quries.GetUserbyEmail;
using Application_Layer.CQRS.User.Quries.GetUserbyid;
using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.Login
{
    public record LoginOrchestrator(int userid,string UserEmial, string UserPassword):IRequest<RequestRespones<AuthModleDto>>;

    public class LoginOrchestratorHandler:IRequestHandler<LoginOrchestrator,RequestRespones<AuthModleDto>>
    {
        private readonly IMediator _mediator;
        private readonly IPasswordHasher passwordHasher;

        public LoginOrchestratorHandler(IMediator mediator, IPasswordHasher passwordHasher)
        {
            _mediator = mediator;
            this.passwordHasher = passwordHasher;
        }

        public async Task<RequestRespones<AuthModleDto>> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new GetUserbyEmailQuery(request.UserEmial));

            if (!user.IsSuccess)
            {
                return RequestRespones<AuthModleDto>.Fail("User not found", 404);
            }

            var mappedUser = new Domain_Layer.Entites.Authantication.User 
            {
                Id=user.Data.Id,
                Username=user.Data.Username,
                Email=user.Data.Email,
                PhoneNumber=user.Data.PhoneNumber,
                Picture=user.Data.Picture,
                UserAddress=user.Data.UserAddress,
            };

            var isPasswordValid = passwordHasher.Verify(mappedUser, request.UserPassword);

            if (!isPasswordValid)
            {
                return RequestRespones<AuthModleDto>.Fail("email or password incorect", 401);
            }

            var UserRoles = user.Data.UserRoles.Select(r => r.RoleName).Distinct().ToList();

           // var UserRoles= await _mediator.Send(new GetRolesByUserIdQury(user.Data.Id));

            var UserPermissions = user.Data.UserPermissions.Select(p => p.PermissionName).Distinct().ToList();

            //var UserPermissions=await _mediator.Send(new GetPermissionsByUseridQuery(user.Data.Id));

            


        }
    }



}
