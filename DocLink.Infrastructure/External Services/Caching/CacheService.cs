using DocLink.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DocLink.Infrastructure.External_Services.Caching
{
	public class CacheService : ICacheService
	{
		private IMemoryCache _cache;

		public CacheService(IMemoryCache cache)
		{
			_cache = cache;
		}

		public T? GetData<T>(string key)
		{

			var data = _cache.Get<T>(key)!;
			return data;
		}

		public void RemoveData(string Key)
		{
			_cache.Remove(Key);
		}


		public bool SetData<T>(string Key, T Value, TimeSpan ExpirationTime)
		{
			_cache.Set(Key, Value, ExpirationTime);
			bool isCached = _cache.TryGetValue(Key, out T _);
			return isCached;
		}
	}
}
