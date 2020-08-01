using RealEstate.Common.Entities.Common;
using RealEstate.DataAccess;
using RealEstate.DataAccess.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Common
{
	public class ContactUsBusinessRule : BaseBusinessRule<ContactUs>
	{
		public ContactUsBusinessRule() : base()
        {
            UnitOfWork = new AppUnitOfWork();
        }
		public ContactUsBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
		 
	}
}
