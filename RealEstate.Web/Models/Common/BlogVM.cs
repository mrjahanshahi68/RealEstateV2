using System;
using System.Collections.Generic;

namespace RealEstate.Web.Models.Common
{
	public class BlogVM
	{
		public int ID { get; set; }
		public string Title { get; set; }
		public string UrlTitle { get; set; }
		public string Summery { get; set; }
		public string HashKey { get; set; }
		public string Body { get; set; }
		public string MetaDescription { get; set; }
		public string SlugUrl { get; set; }
		public int? ViewNumber { get; set; }
		public int? Sort { get; set; }
		public string Discriminator { get; set; }
		public short? CategoryId { get; set; }
		public string CategoryName { get; set; }
		public string Image { get; set; }
		public string ImagePath { get; set; }
		public bool IsActive { get; set; }
		public bool IsDeleted { get; set; }
		public List<AttachmentVM> Attachments { get; set; }
		public DateTime PublishDateTime { get; set; }
		public DateTime InsertDateTime { get; set; }
		public string RegisteredBy { get; set; }
	}
}