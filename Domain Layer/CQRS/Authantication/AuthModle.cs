using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.CQRS.Authantication
{
    public class AuthModle
    {
        public bool IsAuthenticated { get; set; }

        public string? Token { get; set; } = default!;
        public string? RefreshToken { get; set; } = default!;

         public DateTime? TokenExpiration { get; set; }

        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
