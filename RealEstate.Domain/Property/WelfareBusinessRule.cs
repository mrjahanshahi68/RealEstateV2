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
	public class WelfareBusinessRule : BaseBusinessRule<Welfare>
	{
		public WelfareBusinessRule() : base()
        {
            UnitOfWork = new AppUnitOfWork();
        }
		public WelfareBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
		public Welfare FindWelfareByName(string WelfareName)
		{
			return Queryable().Where(e => e.Name == WelfareName).SingleOrDefault();
		}
		 
	}
}
