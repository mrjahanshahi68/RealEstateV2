using System;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Common.Entities.Common
{
	public class Blog : LoggableEntity, ILogicalDeletable
	{
		public string Title { get; set; }
		public string Summery { get; set; }
		public string Body { get; set; }
		public string MetaDescription { get; set; }
		public string SlugUrl { get; set; }
		public int? ViewNumber { get; set; }
		public int? Sort { get; set; }
		public string Discriminator { get; set; }
		public short CategoryId { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public DateTime PublishDateTime { get; set; }
	}
}
