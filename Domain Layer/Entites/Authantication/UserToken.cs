using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Authantication
{
    public class UserToken:BaseEntity
    {
        public User User { get; set; }
        public int UserId { get; set; }

        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
