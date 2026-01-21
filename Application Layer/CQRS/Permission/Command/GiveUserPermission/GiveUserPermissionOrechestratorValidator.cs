using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Permission.Command.GiveUserPermission
{
    public class GiveUserPermissionOrechestratorValidator:AbstractValidator<GiveUserPermissionOrechestrator>
    {
        public GiveUserPermissionOrechestratorValidator()
        {
            // Validate User ID
            RuleFor(x => x.userid)
                .GreaterThan(0).WithMessage("User ID must be greater than zero.");

            // Validate the list of IDs
            RuleFor(x => x.PermssionsIds)
                .NotNull().WithMessage("Permission list cannot be null.")
                .Must(ids => ids != null && ids.Any()).WithMessage("At least one permission ID must be provided.");

            // Validate each individual ID in the list
            RuleForEach(x => x.PermssionsIds)
                .GreaterThan(0).WithMessage("Invalid Permission ID found: {PropertyValue}. IDs must be greater than zero.");

            // Optional: Ensure no duplicate IDs are sent in the same request
            RuleFor(x => x.PermssionsIds)
                .Must(ids => ids.Distinct().Count() == ids.Count())
                .WithMessage("The request contains duplicate Permission IDs.");
        }
    }
}
