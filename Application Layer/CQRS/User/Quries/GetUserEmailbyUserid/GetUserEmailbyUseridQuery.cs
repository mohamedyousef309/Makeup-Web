using Domain_Layer.DTOs.UserDTOS;
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

namespace Application_Layer.CQRS.User.Quries.GetUserEmailbyUserid
{
    public record GetUserEmailbyUseridQuery(int userid): IRequest<RequestRespones<UserToreturn>>;

    public class GetUserEmailbyUseridQueryHandler : IRequestHandler<GetUserEmailbyUseridQuery, RequestRespones<UserToreturn>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> userRepository;
        public GetUserEmailbyUseridQueryHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<RequestRespones<UserToreturn>> Handle(GetUserEmailbyUseridQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByCriteriaQueryable(x => x.Id == request.userid)
                .Select(x => new UserToreturn
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    UserAddress= x.UserAddress,
                    Username=x.Username,
                    

                })
                .FirstOrDefaultAsync();
            if (user == null)
            {
                return RequestRespones<UserToreturn>.Fail("User not found", 404);
            }
            return RequestRespones<UserToreturn>.Success(user);
        }
    }



}
