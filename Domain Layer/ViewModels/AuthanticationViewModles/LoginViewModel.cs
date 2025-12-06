using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.AuthanticationViewModles
{
    public class LoginViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
    }
}
