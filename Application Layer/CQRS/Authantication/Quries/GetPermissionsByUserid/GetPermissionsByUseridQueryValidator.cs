using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Quries.GetPermissionsByUserid
{
    public class GetPermissionsByUseridQueryValidator:AbstractValidator<GetPermissionsByUseridQuery>
    {
        public GetPermissionsByUseridQueryValidator()
        {

            RuleFor(x => x.userid)
    .NotNull().WithMessage("User ID is required.")
    .GreaterThan(0).WithMessage("User ID must be greater than 0.");
        }
    }
}
