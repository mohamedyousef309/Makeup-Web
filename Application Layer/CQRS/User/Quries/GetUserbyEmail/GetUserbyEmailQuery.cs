using Domain_Layer.DTOs.AthanticationDtos;
using Domain_Layer.DTOs.UserDTOS;
using Domain_Layer.Interfaces.Repositryinterfaces;
using Domain_Layer.Respones;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Quries.GetUserbyEmail
{
    public record GetUserbyEmailQuery(string email):IRequest<RequestRespones<UserToReturnWithRolsDTo>>;

    public class GetUserbyEmailQueryHandler : IRequestHandler<GetUserbyEmailQuery, RequestRespones<UserToReturnWithRolsDTo>>
    {
        private readonly IGenaricRepository<Domain_Layer.Entites.Authantication.User> _genaricRepository;

        public GetUserbyEmailQueryHandler(IGenaricRepository<Domain_Layer.Entites.Authantication.User> genaricRepository)
        {
            this._genaricRepository = genaricRepository;
        }
        public async Task<RequestRespones<UserToReturnWithRolsDTo>> Handle(GetUserbyEmailQuery request, CancellationToken cancellationToken)
        {
            var user = await _genaricRepository.GetByCriteriaQueryable(x=>x.Email==request.email).Select(u => new UserToReturnWithRolsDTo
            {
                Id = u.Id,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,

            }).FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                return RequestRespones<UserToReturnWithRolsDTo>.Fail("User not found", 404);

            }

            return RequestRespones<UserToReturnWithRolsDTo>.Success(user);


        }
    }



}
