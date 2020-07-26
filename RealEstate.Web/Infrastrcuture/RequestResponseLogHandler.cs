using RealEstate.Common;
using RealEstate.Web.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace RealEstate.Web.Infrastrcuture
{
    public class RequestResponseLogHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var logMetadata = new RequestResponseLogMetadata
            {
                ApiActionName = request.RequestUri.GetApiAction()
            };
            logMetadata = BuildRequestMetadata(logMetadata,request);
            var response = await base.SendAsync(request, cancellationToken);
            logMetadata = BuildResponseMetadata(logMetadata, response);
            await SendToLog(logMetadata);
            return response;
        }
        
        private RequestResponseLogMetadata BuildRequestMetadata(RequestResponseLogMetadata logMetadata,HttpRequestMessage request)
        {
            
            var content = request.Content.ReadAsStringAsync();
            var result = content.Result;

            logMetadata.RequestContent = result;
            logMetadata.RequestMethod = request.Method.Method;
            logMetadata.RequestTimestamp = DateTime.Now;
            logMetadata.RequestUri = request.RequestUri.ToString();

            return logMetadata;
        }
        private RequestResponseLogMetadata BuildResponseMetadata(RequestResponseLogMetadata logMetadata, HttpResponseMessage response)
        {
            var content = response.Content.ReadAsStringAsync();
            var result = content.Result;

            logMetadata.ResponseContent = result;
            logMetadata.ResponseStatusCode = response.StatusCode;
            logMetadata.ResponseTimestamp = DateTime.Now;
            logMetadata.ResponseContentType = response.Content.Headers.ContentType.MediaType;

            return logMetadata;
        }
        private  Task<bool> SendToLog(RequestResponseLogMetadata logMetadata)
        {
            new Thread(new ThreadStart(() =>
            {
                LogManager.WriteLog<FileLogger>(Newtonsoft.Json.JsonConvert.SerializeObject(logMetadata), $"{logMetadata.ApiActionName} ( Request-{logMetadata.RequestTimestamp?.ToTimeStamp()} Response-{logMetadata.ResponseTimestamp?.ToTimeStamp()} ).txt");
            })).Start();

            return Task.FromResult(true);
        }
    }
}