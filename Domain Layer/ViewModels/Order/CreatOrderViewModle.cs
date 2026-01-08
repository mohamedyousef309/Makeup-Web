using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.ViewModels.Order
{
    public class CreatOrderViewModle
    {
      

       
        public string? BuyerEmail { get; set; }

      
        [RegularExpression(@"^01[0-2,5]{1}[0-9]{8}$",
            ErrorMessage = "Invalid Egyptian phone number.")]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
    }
}
