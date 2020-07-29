using RealEstate.Common.Entities.Property;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Property
{
	public class PropertyInfoBusinessRule : BaseBusinessRule<PropertyInfo>
	{
		public PropertyInfoBusinessRule() : base()
        {
			UnitOfWork = new AppUnitOfWork();
		}
		public PropertyInfoBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
	}
}
