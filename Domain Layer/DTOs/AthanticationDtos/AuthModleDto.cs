using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.DTOs.AthanticationDtos
{
    public class AuthModleDto
    {
        public bool IsAuthenticated { get; set; }
        public string? Token { get; set; }
        public DateTime? TokenExpiresOn { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenExpiration { get; set; }
    }
}
