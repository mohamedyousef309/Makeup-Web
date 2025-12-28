using Domain_Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.ServiceInterfaces
{
    public interface IEMailService
    {
        public Task SendEmail(EmailDTo Email);
    }
}
