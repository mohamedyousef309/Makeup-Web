using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Authantication
{
    public class RefreshTokens:BaseEntity
    {
        public int userid { get; set; }=default!;   

        public User User { get; set; }=default!;  
        

       public string Token { get; set; }=default!;
        public bool IsUsed { get; set; } = false; // لمنع إعادة الاستخدام


        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
