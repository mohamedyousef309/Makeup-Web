using BCrypt.Net;
using Domain_Layer.Entites.Authantication;
using Domain_Layer.Interfaces.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class PasswordHasher:IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(User user, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }
    }
}
