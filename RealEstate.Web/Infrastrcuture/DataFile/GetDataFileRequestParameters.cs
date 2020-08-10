using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealEstate.Web.Infrastrcuture.DataFile
{
	public class GetDataFileRequestParameters<TViewModel, TFileData>
	{
		public TViewModel ViewModel { get; set; }
		public List<DataFileInfo<TFileData>> Files { get; set; }
	}
}