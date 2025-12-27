using Domain_Layer.Entites.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Layer.Interfaces.Repositryinterfaces
{
    public interface IBasketRepository
    {
        Task<UserCart?> GetCustomerBasket(string USerId);



        Task<UserCart?> UpdateOrCreateCustomerBasket(UserCart basket);

        Task<bool> DeleteCustomerBasket(string basketId);
    }
}
