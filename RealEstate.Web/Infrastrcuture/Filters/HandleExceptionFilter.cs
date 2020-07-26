using RealEstate.Common;
using RealEstate.Common.Exceptions;
using RealEstate.Web.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.ServiceModel.Channels;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using static RealEstate.Common.AppEnums;

namespace RealEstate.Web.Infrastrcuture.Filters
{
    public class HandleExceptionFilter : ExceptionFilterAttribute
    {
        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            string exceptionMessage = string.Empty;
            var errorMessages = new List<string>();
            var resultCode = ResultCode.InternalServerError;
            var exception = actionExecutedContext.Exception;

            if (exception is ValidationModelException)
            {
                var validationModelException = exception as ValidationModelException;
                errorMessages = validationModelException.Messages;
                resultCode = ResultCode.ValidationError;
            }
            else
            {
                if (exception.InnerException == null)
                {
                    exceptionMessage = actionExecutedContext.Exception.Message;
                }
                else
                {
                    exceptionMessage = exception.InnerException.Message;
                }
                errorMessages.Add(exceptionMessage);
            }
            
            new Thread(new ThreadStart(() =>
            {
                var request = actionExecutedContext.Request;
                var exepurl = request.RequestUri.ToString();
                var exceptionLog = new Common.Entities.Log.Exception
                {
                    ActionName = request.RequestUri.GetApiAction(),
                    ExceptionMsg = exception.Message,
                    ExceptionSource = exception.StackTrace.ToString(),
                    ExceptionType = exceptionMessage.GetType().Name.ToString(),
                    ExceptionURL = request.RequestUri.ToString(),
                    IPAddress = GetClientIp(request),
                    Logdate = DateTime.Now,
                };
                LogManager.WriteLog<DatabaseLogger>(exceptionLog);
            })).Start();

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent(
                typeof(ApiResult),
                new ApiResult
                {
                    ResultCode = resultCode,
                    Messages = errorMessages,
                }, new JsonMediaTypeFormatter())
            };

            actionExecutedContext.Response = response;

            return Task.FromResult(0);
        }
        private string GetClientIp(HttpRequestMessage request)
        {

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return null;//((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    }
}