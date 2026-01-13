using Domain_Layer.Interfaces.Abstraction;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Commands.EditUserProfile
{
    public record EditUserProfileCommnad(string Username, string Email, string UserAddress, string PhoneNumber) :ICommand<RequestRespones<bool>>;

    public class EditUserProfileCommnadHandler : IRequestHandler<EditUserProfileCommnad, RequestRespones<bool>>
    {
        public Task<RequestRespones<bool>> Handle(EditUserProfileCommnad request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
