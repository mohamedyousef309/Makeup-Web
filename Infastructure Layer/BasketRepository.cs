using Domain_Layer.Entites.Authantication;
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
        public BasketRepository(IConnectionMultiplexer context)
        {
            _DbContext = context.GetDatabase();
        }
       

        public async Task<UserCart?> GetCustomerBasket(string USerId)
        {
            var basket = await _DbContext.StringGetAsync(RedisKeys.BasketKey(USerId));

            if (basket.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<UserCart>(basket.ToString());
        }

      

        public async Task<UserCart?> UpdateOrCreateCustomerBasket(UserCart basket)
        {
            string key = RedisKeys.BasketKey(basket.Id);
            var jsonbasket = JsonSerializer.Serialize(basket);

            var isCreated = await _DbContext.StringSetAsync(key, jsonbasket, TimeSpan.FromDays(10));
            if (isCreated)
            {
                return await GetCustomerBasket(basket.Id);
            }
            return null;

        }

        public async Task<bool> DeleteCustomerBasket(string userId)
        {
            // يجب حذف المفتاح بالـ Prefix أيضاً
            return await _DbContext.KeyDeleteAsync(RedisKeys.BasketKey(userId));
        }


    }

     class RedisKeys
    {
        // ميثود ثابتة لإنتاج المفتاح
        public static string BasketKey(string userId) => $"basket:{userId}";
    }
}
