using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Common
{
	public class AttachmentVM
	{
		public string HashKey { get; set; }
		public ObjectType ObjectType { get; set; }
		public int ObjectId { get; set; }
		public string FileName { get; set; }
		public float FileSize { get; set; }
		public string ContentType { get; set; }
		//public bool IsDeleted { get; set; }
	}
}