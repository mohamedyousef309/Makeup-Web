using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.User.Quries.GetUserbyEmail
{
    public class GetUserbyEmailQueryValidator: AbstractValidator<GetUserbyEmailQuery>
    {
        public GetUserbyEmailQueryValidator()
        {
            
        }
    }
}
