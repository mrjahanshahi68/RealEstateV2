using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models
{
	public class FilterQueryRsponse
	{
		public int TotalCount { get; set; }
		public object Records { get; set; }
	}
}