using Domain_Layer.Entites.Basket;
using Domain_Layer.Interfaces.Repositryinterfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infastructure_Layer
{
    public class BasketRepository : IBasketRepository
    {
        IDatabase _DbContext;
        public BasketRepository(ConnectionMultiplexer context)
        {
            _DbContext = context.GetDatabase();
        }
       

        public async Task<UserCart?> GetCustomerBasket(string basketId)
        {
            var basket = await _DbContext.StringGetAsync(basketId);

            if (basket.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<UserCart>(basket.ToString());
        }

        public async Task<UserCart?> UpdateOrCreateCustomerBasket(UserCart basket)
        {
            var jsonbasket = JsonSerializer.Serialize(basket);

            var isCreated = await _DbContext.StringSetAsync(basket.Id, jsonbasket, TimeSpan.FromDays(30));
            if (isCreated)
            {
                return await GetCustomerBasket(basket.Id);
            }
            return null;

        }

        public async Task<bool> DeleteCustomerBasket(string basketId)
        => await _DbContext.KeyDeleteAsync(basketId);
    }
}
