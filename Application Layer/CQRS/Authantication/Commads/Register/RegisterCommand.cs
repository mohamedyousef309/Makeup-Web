using Domain_Layer.Constants;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Interfaces.ServiceInterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.Register
{
    public record RegisterCommand(string Email, string Password, string PhoneNumber,string UserAddress, IFormFile? Image) :IRequest<RequestRespones<bool>>;

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;
        private readonly IPasswordHasher passwordHasher;
        private readonly IAttachmentService attachmentService;

        public RegisterCommandHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User > genaricRepository,IPasswordHasher passwordHasher,IAttachmentService attachmentService)
        {
            this.genaricRepository = genaricRepository;
            this.passwordHasher = passwordHasher;
            this.attachmentService = attachmentService;
        }
        public async Task<RequestRespones<bool>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var HashedPassword = passwordHasher.Hash(request.Password);

                var validationResult = await ValidateRegister(request.Email, request.PhoneNumber);

                if (!validationResult.IsSuccess)
                {
                    return validationResult;

                }

                var newUser = new Domain_Layer.Entites.Authantication.User
                {
                    Email = request.Email,
                    PasswordHash = HashedPassword,
                    PhoneNumber = request.PhoneNumber,
                    UserAddress = request.UserAddress,
                    Username = request.Email.Split('@')[0],


                };

                if (request.Image!=null)
                {
                    newUser.Picture = attachmentService.UploadImage(request.Image, "Images")??"null";
                }
                newUser.UserRoles = new List<Domain_Layer.Entites.Authantication.UserRole>
                {
                new UserRole { Userid = newUser.Id, Roleid = RoleConstants.User_id}
                };

                await genaricRepository.addAsync(newUser);
                await genaricRepository.SaveChanges();
                return RequestRespones<bool>.Success(true);

            }
            catch (Exception ex)
            {
                return RequestRespones<bool>.Fail("Error while registering",400);

                throw;
            }
           
        }

        private async Task<RequestRespones<bool>> ValidateRegister(string Email,string phone)
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
