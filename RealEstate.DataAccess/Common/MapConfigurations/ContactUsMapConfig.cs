using RealEstate.Common.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static RealEstate.Common.AppConstants;

namespace RealEstate.DataAccess.Common.MapConfigurations
{
	public class ContactUsMapConfig : LoggableEntityMapConfig<ContactUs>
	{
		public ContactUsMapConfig()
		{
			Property(e => e.Title);
			Property(e => e.LinkUrlInstagram);
			Property(e => e.LinkUrlWhatsapp);
			Property(e => e.LinkUrlTelegram);
			Property(e => e.LinkUrlPinterest);
			Property(e => e.LinkUrlGoogle);
			Property(e => e.MobileNumber);
			Property(e => e.PhoneNumber);
			Property(e => e.Address);
			Property(e => e.IsDeleted);

		ToTable("ContactUses");
		}
	}
}
