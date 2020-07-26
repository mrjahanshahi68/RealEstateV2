using RealEstate.Common.Entities.Security;
using RealEstate.DataAccess;
using RealEstate.DataAccess.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Security
{
	public class UserBusinessRule : BaseBusinessRule<User>
	{
		public UserBusinessRule() : base()
        {
            UnitOfWork = new AppUnitOfWork();
        }
		public UserBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
		public User FindUserByUserName(string userName)
		{
			return Queryable().Where(e => e.UserName == userName).SingleOrDefault();
		}
		 
	}
}
