using RealEstate.Common.Entities.Customer;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Customer
{
	public class CustomerInfoBusinessRule : BaseBusinessRule<CustomerInfo>
	{
		public CustomerInfoBusinessRule() : base()
        {
			UnitOfWork = new AppUnitOfWork();
		}
		public CustomerInfoBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
	}
}
