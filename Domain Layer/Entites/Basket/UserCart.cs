using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Entites.Basket
{
    public class UserCart
    {
        public string Id { get; set; }

        public int UserId { get; set; }
        public ICollection<CartItem> Items { get; set; }
    }
}
