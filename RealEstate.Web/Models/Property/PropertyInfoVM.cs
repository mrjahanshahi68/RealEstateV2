using RealEstate.Common.Entities.Common;
using RealEstate.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Models.Property
{
	public class PropertyInfoVM
	{
		public int ID { get; set; }
		public int? PropertyTypeId { get; set; }
		public int? DocumentTypeId { get; set; }
		//public int OwnerId { get; set; }

		public string HashKey { get; set; }
		public string PropertyCode { get; set; }
		public string Title { get; set; }
		public string UrlTitle { get; set; }

		public PropertyStatus? Status { get; set; }
		public TransactionType? Type { get; set; }
		public string TransactionTypeName { get; set; }
		public double? BuildingArea { get; set; }
		public double? LandingArea { get; set; }
		public int? BathRoom { get; set; }
		public int? BedRoom { get; set; }
		public int? Floor { get; set; }
		public int? FloorNumber { get; set; }
		public int? Apartment { get; set; }
		public string Position { get; set; }
		public string PropertyView { get; set; }
		public int? ConstructorYear { get; set; }
		//public string Welfares { get; set; }

		public int? StateId { get; set; }
		public int? CityId { get; set; }
		public int? RegionId { get; set; }
		public string Address { get; set; }
		public string Description { get; set; }

		public decimal? SalePrice { get; set; }
		public decimal? MortgagePrice { get; set; }
		public decimal? RentalPrice { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Tel { get; set; }
		public string FullName { get; set; }

		public string ReagentFirstName { get; set; }
		public string ReagentLastName { get; set; }
		public string ReagentTel { get; set; }
		public string ReagentFullName { get; set; }


		public string SlideImage { get; set; }
		public string SlideImagePath { get; set; }
		public string CoverImage { get; set; }
		public string CoverImagePath { get; set; }
		//public bool IsLuxury { get; set; }
		//public bool IsShowInSlide { get; set; }

		public bool IsDeleted { get; set; }

		public List<WelfareVM> Welfares { get; set; }

		public string WelfaresString { get; set; }
		public string PropertyTypeName { get; set; }
		public string DocumentTypeName { get; set; }
		public string RegionName { get; set; }
		public DateTime RegisterDate { get; set; }
		//public List<Attachment> Attachments { get; set; }
		public List<AttachmentVM> Attachments { get; set; }
		//public List<ContentInfo> Attachments { get; set; }
		public string FullText { get; set; }
		public int RegisterBy { get; set; }

	}
}