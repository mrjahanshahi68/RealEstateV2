using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Cache
{
	public class WebCacheProvider : ICache
	{
		private static Dictionary<object, object> cacheItems = new Dictionary<object, object>();
		public object GetValue(object key)
		{
			if (cacheItems.ContainsKey(key))
				return cacheItems[key];
			return null;
		}

		public bool Remove(object key)
		{
			if (cacheItems.ContainsKey(key))
				return cacheItems.Remove(key);
			return false;
		}

		public void SetValue(object key, object value)
		{
			if (!cacheItems.ContainsKey(key))
				cacheItems.Add(key, value);
		}
	}
}