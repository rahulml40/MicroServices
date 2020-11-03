using Newtonsoft.Json;
using RedisDemo.Data;
using RedisDemo.Entities;
using RedisDemo.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisDemo.Repositories
{
    public class BasketRepositories : IBasketRepositories
    {
        private readonly IBasketContext _context;

        public BasketRepositories(IBasketContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            return await _context
                          .Redis
                          .KeyDeleteAsync(userName);
        }

        public async Task<BasketCart> GetBasket(string userName)
        {   
            var basket = await _context
                            .Redis
                            .StringGetAsync(userName);
            if (basket.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basket)
        {
            var updated = await _context
                              .Redis
                              .StringSetAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            if (!updated)
            {
                return null;
            }
            return await GetBasket(basket.UserName);
        }
    }
}
