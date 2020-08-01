using RealEstate.Common.Entities.Common;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Common
{
	public class StateBusinessRule:BaseBusinessRule<State>
	{
		public StateBusinessRule() : base()
        {
			UnitOfWork = new AppUnitOfWork();
		}
		public StateBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
	}
}
