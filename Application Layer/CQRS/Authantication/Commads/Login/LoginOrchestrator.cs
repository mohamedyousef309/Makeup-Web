using Application_Layer.CQRS.Authantication.Quries.GetPermissionsByUserid;
using Application_Layer.CQRS.Authantication.Quries.GetRolesByUserid;
using Application_Layer.CQRS.User.Quries.GetUserbyEmail;
using Application_Layer.CQRS.User.Quries.GetUserbyid;
using Application_Layer.CQRS.User.Quries.UserToReturnForLogin;
using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.Login
{
    public record LoginOrchestrator(string UserEmial, string UserPassword):IRequest<RequestRespones<AuthModleDto>>;

    public class LoginOrchestratorHandler:IRequestHandler<LoginOrchestrator,RequestRespones<AuthModleDto>>
    {
        private readonly IMediator _mediator;
        private readonly IPasswordHasher passwordHasher;
        private readonly IAuthService authService;

        public LoginOrchestratorHandler(IMediator mediator, IPasswordHasher passwordHasher,IAuthService authService)
        {
            _mediator = mediator;
            this.passwordHasher = passwordHasher;
            this.authService = authService;
        }

        public async Task<RequestRespones<AuthModleDto>> Handle(LoginOrchestrator request, CancellationToken cancellationToken)
        {
            var user = await _mediator.Send(new UserToReturnForLoginByEmailQuery(request.UserEmial));

            if (!user.IsSuccess)
            {
                return RequestRespones<AuthModleDto>.Fail(user.Message, 404);
            }

           

            var isPasswordValid = passwordHasher.Verify(user.Data, request.UserPassword);

            if (!isPasswordValid)
            {
                return RequestRespones<AuthModleDto>.Fail("email or password incorect", 401);
            }


            var UserRolesRespone= await _mediator.Send(new GetRolesByUserIdQury(user.Data.Id));

            var UserRoles = UserRolesRespone.Data?.Select(r => new Role
            {
                Id = r.RoleId,
                Name = r.RoleName
            }).ToList() ?? Enumerable.Empty<Role>();


            var UserPermissionsRespone =await _mediator.Send(new GetPermissionsByUseridQuery(user.Data.Id));

            var UserPermissions = UserPermissionsRespone.Data?.Select(r => new Permissions
            {
                Id = r.PermissionId,
                Name = r.PermissionName
            }).ToList() ?? Enumerable.Empty<Permissions>();

            var token = await authService.GenerateTokensAsync(user.Data, UserRoles, UserPermissions);

            return RequestRespones<AuthModleDto>.Success(token);

        }
    }



}
