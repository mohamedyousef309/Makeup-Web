using Domain_Layer.Entites.Authantication;
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

namespace Application_Layer.CQRS.Authantication.Commads.ResetPassword
{
    public record ResetPasswordCommand(string UserEmail , string NewPassword) :ICommand<RequestRespones<bool>>;

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;

        public ResetPasswordCommandHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
           var user = await genaricRepository.GetByCriteriaQueryable(x=>x.Email==request.UserEmail).Select(x=>new Domain_Layer.Entites.Authantication.User 
           {
               Id=x.Id,
               PasswordHash=x.PasswordHash,
           }).FirstOrDefaultAsync();

            var newHashedPassword = GeneratePasswordHash(request.NewPassword);
            if (user == null) 
            {
                return RequestRespones<bool>.Fail("There is no user with this email",404);
            }

            user.PasswordHash = newHashedPassword;

            genaricRepository.SaveInclude(user, nameof(Domain_Layer.Entites.Authantication.User.PasswordHash));

            await genaricRepository.SaveChanges();

           return RequestRespones<bool>.Success(true,200,"Password has been reset successfully");


        }

        private string GeneratePasswordHash(string realPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(realPassword);
        }
    }
}
