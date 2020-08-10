using RealEstate.Common.Entities.Property;
using RealEstate.DataAccess;
using RealEstate.DataAccess.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Property
{
	public class PropertyDemandBusinessRule : BaseBusinessRule<PropertyDemand>
	{
		public PropertyDemandBusinessRule() : base()
        {
            UnitOfWork = new AppUnitOfWork();
        }
		public PropertyDemandBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
		 
	}
}
