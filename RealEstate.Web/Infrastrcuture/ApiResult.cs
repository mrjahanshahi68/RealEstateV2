using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Infrastrcuture
{
	public class ApiResult
	{
		public ResultCode ResultCode { get; set; }
		public List<string> Messages { get; set; }
		public object Data { get; set; }
	}
}