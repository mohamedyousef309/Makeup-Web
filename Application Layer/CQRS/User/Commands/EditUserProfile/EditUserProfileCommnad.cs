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

namespace Application_Layer.CQRS.User.Commands.EditUserProfile
{
    public record EditUserProfileCommnad(int userid,string Username, string Email, string UserAddress, string PhoneNumber) :ICommand<RequestRespones<bool>>;

    public class EditUserProfileCommnadHandler : IRequestHandler<EditUserProfileCommnad, RequestRespones<bool>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository;

        public EditUserProfileCommnadHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this.genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<bool>> Handle(EditUserProfileCommnad request, CancellationToken cancellationToken)
        {
            var user= await genaricRepository.GetByCriteriaQueryable(x=>x.Id==request.userid).FirstOrDefaultAsync(cancellationToken);

            if (user==null)
            {
             return RequestRespones<bool>.Fail("User not found",404);
            }

            user.Username=request.Username;
            user.Email=request.Email;
            user.UserAddress=request.UserAddress;
            user.PhoneNumber=request.PhoneNumber;

            genaricRepository.SaveInclude(user,nameof(user.Username),
                nameof(user.Email),
                nameof(user.UserAddress),
                nameof(user.PhoneNumber));

             await genaricRepository.SaveChanges();

            return RequestRespones<bool>.Success(true,200);



        }
    }
}
