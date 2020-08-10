using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models.Security
{
	public class MenuItemResponse
	{
		public string Guid { get; set; }
		public string Name { get; set; }
		public string Link { get; set; }
		public string Icon { get; set; }
		public List<MenuItemResponse> MenuItems { get; set; }
	}

}