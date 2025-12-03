using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.CQRS.Authantication.Commads.Login
{
    public class LoginCommendValidator: AbstractValidator<LoginCommend>
    {
        public LoginCommendValidator()
        {
            
        }
    }
}
