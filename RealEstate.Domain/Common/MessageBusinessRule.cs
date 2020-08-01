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
	public class MessageBusinessRule : BaseBusinessRule<Message>
	{
		public MessageBusinessRule() : base()
        {
            UnitOfWork = new AppUnitOfWork();
        }
		public MessageBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }

		public Message FindMessageById(int id)
		{
			return Queryable().Where(e => e.ID == id).SingleOrDefault();
		}
	}
}
