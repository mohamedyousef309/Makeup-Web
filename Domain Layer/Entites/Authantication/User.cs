using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Authantication
{
    public class User:BaseEntity
    {
        public string Username { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public string Picture { get; set; } = default!;
        public string UserAddress { get; set; } = default!;
        public bool EmailConfirmed { get; set; } = false;
        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEnd { get; set; }

        public IEnumerable<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();

        public IEnumerable<RefreshTokens> refreshTokens= new HashSet<RefreshTokens>();

       public  IEnumerable<UserPermissions> userPermissions= new HashSet<UserPermissions>();

        public IEnumerable<UserToken> userTokens= new HashSet<UserToken>();
    }
}
