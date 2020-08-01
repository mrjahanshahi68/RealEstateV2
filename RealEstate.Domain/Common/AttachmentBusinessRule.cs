using RealEstate.Common.Entities.Common;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Common
{
	public class AttachmentBusinessRule : BaseBusinessRule<Attachment>
	{
		public AttachmentBusinessRule() : base()
        {
			UnitOfWork = new AppUnitOfWork();
		}
		public AttachmentBusinessRule(IUnitOfWork unitOfWork) : base(unitOfWork) { }
	}
}
