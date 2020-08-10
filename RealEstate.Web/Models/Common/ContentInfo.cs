using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;

namespace RealEstate.Web.Models.Common
{
	public class ContentInfo
	{
		public MediaTypeHeaderValue ContentType { get; set; }
		public string FileName { get; set; }
		public string Size { get; set; }
		public byte[] Content { get; set; }
	}
}