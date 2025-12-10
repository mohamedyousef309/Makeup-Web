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

namespace Application_Layer.CQRS.Authantication.Commads.Register
{
    public record RegisterCommand(string Email, string Password, string PhoneNumber,string UserAddress) :IRequest<RequestRespones<bool>>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;
        private readonly IPasswordHasher passwordHasher;

        public RegisterCommandHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User > genaricRepository,IPasswordHasher passwordHasher)
        {
            this.genaricRepository = genaricRepository;
            this.passwordHasher = passwordHasher;
        }
        public async Task<RequestRespones<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var HashedPassword = passwordHasher.Hash(request.Password);

            var isdataNotDuplicated= ValidateRegister(request.Email,request.PhoneNumber);

            var UserRoles = new List<Domain_Layer.Entites.Authantication.Role>
            {
                new UserRole {S }
            };

            var newUser = new Domain_Layer.Entites.Authantication.User
            {
                Email = request.Email,
                PasswordHash = HashedPassword,
                PhoneNumber = request.PhoneNumber,
                UserAddress = request.UserAddress,
                Username = request.Email.Split('@')[0],
                UserRoles = UserRoles


            };
            return RequestRespones<bool>.Success(true);
        }

        private async Task< RequestRespones<bool>> ValidateRegister(string Email,string phone)
        {
            var emailExists = await genaricRepository.ExistsAsync(u => u.Email == Email);
            if (emailExists)
            {
                return RequestRespones<bool>.Fail("Email already used", 400);
            }

            var phoneExists = await genaricRepository.ExistsAsync(u => u.PhoneNumber == phone);
            if (phoneExists)
            {
                return RequestRespones<bool>.Fail("Phone number already used", 400);
            }

            return RequestRespones<bool>.Success(true);

        }
    }

    


}
