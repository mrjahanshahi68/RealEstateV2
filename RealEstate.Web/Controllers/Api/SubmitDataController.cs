using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace RealEstate.Web.Controllers.Api
{
	public class SubmitDataController : ApiController
	{
		[HttpPost]
		[AllowAnonymous]
		public HttpResponseMessage Post()
		{
			var fileDataRequestParameters = HttpContext.Current.Request.GetDataFileRequestParameters<SubmitDataVM, FileDataModel>();
			
			return Request.CreateResponse(HttpStatusCode.OK, new { });
		}

		public class DataModel
		{
			public string FirstName { get; set; }
			public string LastName { get; set; }
		}
	}

	public class SubmitDataVM
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

	public class DataFileInfo<TData>
	{
		public HttpPostedFile HttpPostedFile { get; set; }
		public TData FileData { get; set; }
	}

	public class FileDataModel
	{
		public string ExtraInfoString { get; set; }
	}

	public class GetDataFileRequestParameters<TViewModel, TFileData>
	{
		public TViewModel ViewModel { get; set; }
		public List<DataFileInfo<TFileData>> Files { get; set; }
	}

	public static class HttpRequestExtensions
	{
		public static GetDataFileRequestParameters<TViewModel, TFileData> GetDataFileRequestParameters<TViewModel, TFileData>(this HttpRequest httpRequest)
		{
			GetDataFileRequestParameters<TViewModel, TFileData> result = new Api.GetDataFileRequestParameters<TViewModel, TFileData>
			{
				Files = new List<DataFileInfo<TFileData>>()
			};

			result.ViewModel = JsonConvert.DeserializeObject<TViewModel>(httpRequest.Form["Parameters"]);
			string[] fileKeys = JsonConvert.DeserializeObject<string[]>(httpRequest.Form["FileKeys"]);
			foreach (string fileKey in fileKeys)
			{
				result.Files.Add(new DataFileInfo<TFileData>
				{
					FileData = JsonConvert.DeserializeObject<TFileData>(httpRequest.Form[$"Data{fileKey}"]),
					HttpPostedFile = httpRequest.Files[fileKey]
				});
			}
			return result;
		}
	}
}
