﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Common.Entities.Common
{
	public class Attachment : LoggableEntity
	{
		public string HashKey { get; set; }
		public ObjectType ObjectType { get; set; }
		public int ObjectId { get; set; }
		public string FileName { get; set; }
		public double? FileSize { get; set; }
		public FileUnit FileUnit { get; set; }
		public string ContentType { get; set; }
		public bool IsDeleted { get; set; }
	}
}
