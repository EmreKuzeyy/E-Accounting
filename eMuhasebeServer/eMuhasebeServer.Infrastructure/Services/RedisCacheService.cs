using eMuhasebeServer.Application.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace eMuhasebeServer.Infrastructure.Services
{
    internal sealed class RedisCacheService : ICacheService
    {
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public T? Get<T>(string key)
        {
            var value = _database.StringGet(key);
            if (value.HasValue)
            {
                var result = JsonSerializer.Deserialize<T?>(value.ToString());
            }

            return default(T?);
        }

        public bool Remove(string key)
        {
            return _database.KeyDelete(key);
        }

        public void RemoveAll()
        {
            List<string> keys = new()
            {
                "cashRegisters",
                "banks",
                "invoices",
                "customers",
                "products"
            };
            foreach (var key in keys)
            {
                _database.KeyDelete(key);
            }
        }

        public void Set<T>(string key, T value, TimeSpan? expire = null)
        {
            var serializedValue = JsonSerializer.Serialize(value);
            _database.StringSet(key, serializedValue, expire);
        }
    }
}
