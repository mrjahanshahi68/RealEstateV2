using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Common.Entities.Common
{
	public class ContactUs : LoggableEntity, ILogicalDeletable
	{
		public string Title { get; set; }
		public string LinkUrlInstagram { get; set; }
		public string LinkUrlWhatsapp { get; set; }
		public string LinkUrlTelegram { get; set; }
		public string LinkUrlPinterest { get; set; }
		public string LinkUrlGoogle { get; set; }
		public string MobileNumber { get; set; }
		public string PhoneNumber { get; set; }
		public string Address { get; set; }
		public bool IsDeleted { get; set; }

	}
}
