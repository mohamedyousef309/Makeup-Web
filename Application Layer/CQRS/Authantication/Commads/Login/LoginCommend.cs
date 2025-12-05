using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
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
    public record LoginCommend(string email, string password):IRequest<RequestRespones<AuthModleDto>>;

    public class LoginCommendHandler : IRequestHandler<LoginCommend, RequestRespones<AuthModleDto>>
    {
        private readonly IPasswordHasher passwordHasher;
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> _genaricRepository;

        public LoginCommendHandler(IPasswordHasher  passwordHasher,IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.passwordHasher = passwordHasher;
            this._genaricRepository = genaricRepository;
        }

        public Task<RequestRespones<AuthModleDto>> Handle(LoginCommend request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        //public async Task<RequestRespones<AuthModleDto>> Handle(LoginCommend request, CancellationToken cancellationToken)
        //{
        //    var user = await _genaricRepository.GetByCriteriaAsync(u => u.Email == request.email);

        //    if (user == null)
        //    {
        //        return RequestRespones<AuthModleDto>.Fail("email or password incorect", 401);

        //    }

        //    var isPasswordValid = passwordHasher.Verify(request.password, user.PasswordHash);

        //    if (!isPasswordValid)
        //    {
        //        return RequestRespones<AuthModleDto>.Fail("email or password incorect", 401);

        //    }




        //}
    }
}
