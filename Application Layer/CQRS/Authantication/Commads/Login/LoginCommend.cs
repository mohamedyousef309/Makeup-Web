using Domain_Layer.CQRS.Authantication;
using Domain_Layer.Respones;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.Login
{
    public record LoginCommend:IRequest<RequestRespones<AuthModleDto>>;
    
    
}
