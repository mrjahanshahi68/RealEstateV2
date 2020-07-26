using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Cache
{
	public static class CacheManager
	{
		private static ICache _provider;
		public static void SetProvider(ICache provider)
		{
			_provider = provider;
		}
		public static void SetValue(string key,object value)
		{
			if (_provider == null) throw new NullReferenceException("Cache provider not set");
			_provider.SetValue(key, value);
		}
		public static object GetValue(string key)
		{
			if (_provider == null) throw new NullReferenceException("Cache provider not set");
			return _provider.GetValue(key);
		}
		public static bool Remove(string key)
		{
			if (_provider == null) throw new NullReferenceException("Cache provider not set");
			return _provider.Remove(key);
		}
		public static bool Conatins(string key)
		{
			if (_provider == null) throw new NullReferenceException("Cache provider not set");
			if (_provider.GetValue(key) != null) return true;
			return false;
		}
	}
}