using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Cache
{
	public interface ICache
	{
		void SetValue(object key, object value);
		object GetValue(object key);
		bool Remove(object key);
	}
}