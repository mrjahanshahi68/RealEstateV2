using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace RealEstate.Web.Infrastrcuture
{
    public class RequestResponseLogMetadata
    {
        public string ApiActionName { get; set; }
        public string RequestContent { get; set; }
        public string RequestContentType { get; set; }
        public string RequestUri { get; set; }
        public string RequestMethod { get; set; }
        public DateTime? RequestTimestamp { get; set; }
        public string ResponseContent { get; set; }
        public string ResponseContentType { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public DateTime? ResponseTimestamp { get; set; }
    }
}