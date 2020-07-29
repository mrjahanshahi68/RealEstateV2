using RealEstate.Common.Entities.Property;
using RealEstate.Web.Models.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RealEstate.Domain;
using RealEstate.Domain.Property;

namespace RealEstate.Web.Controllers.Api
{
    public class PropertyInfoController : BaseApiController<PropertyInfo,PropertyInfoVM>
    {
		protected override IBusinessRule<PropertyInfo> CreateRule()
		{
			return new PropertyInfoBusinessRule();
		}

	}
}
