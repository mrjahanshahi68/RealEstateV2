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
	public class BlogBusinessRule : BaseBusinessRule<Blog>
	{
		public BlogBusinessRule() : base()
        {
            UnitOfWork = new AppUnitOfWork();
        }
		public BlogBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }

		public Blog FindMessageById(int id)
		{
			return Queryable().Where(e => e.ID == id).SingleOrDefault();
		}
	}
}
