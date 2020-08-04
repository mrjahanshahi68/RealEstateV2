using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Models.Common
{
	public class ContentInfo
	{
		public string ContentType { get; set; }
		public string FileName { get; set; }
		public string Size { get; set; }
		public string Content { get; set; }
		public byte[] GetFileAsByte()
		{
			if (!string.IsNullOrWhiteSpace(this.Content))
			{
				var base64 = this.Content.Split(',')[1];
				if (!string.IsNullOrWhiteSpace(base64)) return Convert.FromBase64String(base64);
				return null;
			}
			return null;
		}
	}
}