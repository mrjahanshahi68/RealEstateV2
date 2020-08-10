using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Infrastrcuture.DataFile
{
	public class DataFileInfo<TData>
	{
		public HttpPostedFile HttpPostedFile { get; set; }
		public TData FileData { get; set; }
	}
}